using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Motivo.ApiModels;
using Motivo.Data;
using System.Linq;


namespace Motivo
{
	[ApiController]
	[Route(UserActionsRoutes.Controller)]
	public class UserActionsController : ControllerBase
	{
		protected MotivoDbContext _motivoDbContext;

		/// <summary>
		///     The manager for handling signing in or out our Users
		/// </summary>
		protected SignInManager<MotivoUser> _signInManager;

		/// <summary>
		///     The manager for handling user creation, deletion, search, roles, etc...
		/// </summary>
		protected UserManager<MotivoUser> _userManager;

		public UserActionsController(ILogger<UserActionsController> logger, MotivoDbContext motivoDbContext,
			UserManager<MotivoUser> userManager, SignInManager<MotivoUser> signInManager)
		{
			_motivoDbContext = motivoDbContext;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[AllowAnonymous]
		[Route(UserActionsRoutes.Login)]
		public async Task<ApiResponse<LoginResultApiModel>> LoginAsync([FromBody] LoginCredentialsApiModel loginCredentials)
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
			var user = await _userManager.FindByNameAsync(loginCredentials?.Username);

			// if we failed to found the user
			if (user == null)
				return errorResponse;

			// If user was found, validate password
			var isValidPassword = await _userManager.CheckPasswordAsync(user, loginCredentials.Password);

			if (!isValidPassword)
				return errorResponse;


			return new ApiResponse<LoginResultApiModel>
			{
				// Pass the user's username and the token
				Response = new LoginResultApiModel
				{
					Username = user.UserName,
					Token = user.GenerateJwtToken()
				}
			};
		}

		[AllowAnonymous]
		[Route(UserActionsRoutes.Register)]
		public async Task<ApiResponse<RegisterResultApiModel>> CreateUserAsync([FromBody] RegisterCredentialsApiModel registerCredentials)
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
			var result = await _userManager.CreateAsync(new MotivoUser
			{
				UserName = registerCredentials.Username,
				Email = registerCredentials.Email,
			}, registerCredentials.Password);


			// If the registration was successful
			if (result.Succeeded)
			{
				var userIdentity = await _userManager.FindByNameAsync(registerCredentials.Username);
				return new ApiResponse<RegisterResultApiModel>
				{
					Response = new RegisterResultApiModel
					{
						Username = userIdentity.UserName,
						Email = userIdentity.Email
					}
				};
			}
			// Otherwise it failed
			else
			{
				// Return the failed response
				return new ApiResponse<RegisterResultApiModel>
				{
					ErrorMessage = result.Errors?.AggregateErrors()
				};
			}
		}
	}
}