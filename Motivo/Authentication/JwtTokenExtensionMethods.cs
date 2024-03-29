﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Motivo.Data;
using Motivo.IoC;

namespace Motivo.Authentication
{
    public static class JwtTokenExtensionMethods
    {

        public static string GenerateJwtToken(this MotivoUser user)
        {

            /// <summary>
            /// Generates a Jwt bearer token containing the users username
            /// </summary>
            /// <param name="user">The users details</param>
            /// <returns></returns>

            var claims = new[]
            {
                // Unique ID for this token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),

                // Add user Id so that UserManager.GetUserAsync can find the user based on Id
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // Create the credentials used to generate the token
            var credentials = new SigningCredentials(
                // Get the secret key from configuration
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IoCContainer.Configuration["Jwt:SecretKey"])),
                // Use HS256 algorithm
                SecurityAlgorithms.HmacSha256);

            // Generate the Jwt Token
            var token = new JwtSecurityToken(
                issuer: IoCContainer.Configuration["Jwt:Issuer"],
                audience: IoCContainer.Configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMonths(3)
                );

            // Return the generated token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
