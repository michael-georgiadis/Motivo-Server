using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Motivo.ApiModels;
using Motivo.Data;
using Motivo.IoC;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Motivo.Controllers
{
	[ApiController]
	[Route("motivoApi/[controller]")]
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
		[Route("login")]
		public async Task<ApiResponse<LoginResultApiModel>> LoginAsync([FromBody] LoginCredentialsApiModel loginCredentials)
		{
			var errorResponse = new ApiResponse<LoginResultApiModel>
			{
				// Set Error Message
				ErrorMessage = "Invalid Email or Password"
			};

			// Make sure we have an email
			if (loginCredentials?.Email == null || string.IsNullOrWhiteSpace(loginCredentials.Email))
				return errorResponse;

			// Validate if the user credentials are correct
			var user = await _userManager.FindByEmailAsync(loginCredentials?.Email);

			// if we failed to found the user
			if (user == null)
				return errorResponse;

			// If user was found, validate password
			var isValidPassword = await _userManager.CheckPasswordAsync(user, loginCredentials.Password);

			if (!isValidPassword)
				return errorResponse;

			// At this point we have the correct login credentials
			var username = user.UserName;

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
				new Claim(ClaimsIdentity.DefaultNameClaimType, username),
			};

			var credentials = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IoCContainer.Configuration["Jwt:SecretKey"])),
				SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: IoCContainer.Configuration["Jwt:Issuer"],
				audience: IoCContainer.Configuration["Jwt:Audience"],
				claims: claims,
				signingCredentials: credentials,
				expires: DateTime.Now.AddMonths(3));


			var httpUser = HttpContext.User;


			return new ApiResponse<LoginResultApiModel>
			{
				// Pass the user's username and the token
				Response = new LoginResultApiModel
				{
					FirstName = user.UserName,
					Email = user.Email,
					Token = new JwtSecurityTokenHandler().WriteToken(token)
				}
			};
		}

		[AllowAnonymous]
		[Route("register")]
		public async Task<IActionResult> CreateUserAsync()
		{
			var result = await _userManager.CreateAsync(new MotivoUser
			{
				UserName = "Funnyman322",
				//Email = "mikegeor@outlook.com",
				FirstName = "Michael",
			}, "233V!ckylp");

			if (result.Succeeded)
				return Content("User was created", "text/html");

			return Content("User creation failed", "text/html");
		}
	}
}