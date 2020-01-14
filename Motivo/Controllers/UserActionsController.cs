using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Motivo.ApiModels;
using Motivo.ApiModels.Login;
using Motivo.Authentication;
using Motivo.Data;
using Motivo.Routes;

namespace Motivo.Controllers
{
    [ApiController]
    [Route(UserActionsRoutes.Controller)]
    public class UserActionsController : ControllerBase
    {
        protected MotivoDbContext MotivoDbContext;

        /// <summary>
        ///     The manager for handling signing in or out our Users
        /// </summary>
        protected SignInManager<MotivoUser> SignInManager;

        /// <summary>
        ///     The manager for handling user creation, deletion, search, roles, etc...
        /// </summary>
        protected UserManager<MotivoUser> UserManager;

        public UserActionsController(MotivoDbContext motivoDbContext,
            UserManager<MotivoUser> userManager, SignInManager<MotivoUser> signInManager)
        {
            MotivoDbContext = motivoDbContext;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [AllowAnonymous]
        [Route(UserActionsRoutes.Login)]
        public async Task<ApiResponse<LoginResultApiModel>> LoginAsync(
            [FromBody] LoginCredentialsApiModel loginCredentials)
        {
            var errorResponse = new ApiResponse<LoginResultApiModel>
            {
                // Set Error Message
                ErrorMessage = "Invalid Email or Password"
            };

            // Make sure we have an email
            if (loginCredentials?.Username == null || string.IsNullOrWhiteSpace(loginCredentials.Username))
                return errorResponse;

            // Validate if the user credentials are correct
            var user = await UserManager.FindByNameAsync(loginCredentials?.Username);

            // if we failed to found the user
            if (user == null)
                return errorResponse;

            // If user was found, validate password
            var isValidPassword = await UserManager.CheckPasswordAsync(user, loginCredentials.Password);

            if (!isValidPassword)
                return errorResponse;


            return new ApiResponse<LoginResultApiModel>
            {
                // Pass the user's username and the token
                Response = new List<LoginResultApiModel>
                {
                    new LoginResultApiModel
                    {
                        Username = user.UserName,
                        Token = user.GenerateJwtToken()
                    }
                }
            };
        }

        [AllowAnonymous]
        [Route(UserActionsRoutes.Register)]
        public async Task<ApiResponse<RegisterResultApiModel>> CreateUserAsync(
            [FromBody] RegisterCredentialsApiModel registerCredentials)
        {
            var errorResponse = new ApiResponse<RegisterResultApiModel>
            {
                ErrorMessage = "Please provide all required fields to register for an account"
            };

            if (registerCredentials == null)
                return errorResponse;

            // Check if any of the fields are null
            if (string.IsNullOrWhiteSpace(registerCredentials.Username) ||
                string.IsNullOrWhiteSpace(registerCredentials.Email) ||
                string.IsNullOrWhiteSpace(registerCredentials.Password))
                return errorResponse;

            // Try and create user
            var result = await UserManager.CreateAsync(new MotivoUser
            {
                UserName = registerCredentials.Username,
                Email = registerCredentials.Email
            }, registerCredentials.Password);


            // If the registration was successful
            if (!result.Succeeded)
                return new ApiResponse<RegisterResultApiModel>
                {
                    ErrorMessage = result.Errors?.AggregateErrors()
                };

            var userIdentity = await UserManager.FindByNameAsync(registerCredentials.Username);
            return new ApiResponse<RegisterResultApiModel>
            {
                Response = new List<RegisterResultApiModel>
                {
                    new RegisterResultApiModel
                    {
                        Username = userIdentity.UserName,
                        Email = userIdentity.Email
                    }
                }
            };
        }
    }
}