using System;
using System.Collections.Specialized;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Motivo.Data;
using Motivo.IoC;

namespace Motivo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			IoCContainer.Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<MotivoDbContext>(options =>
				{
					options.UseSqlServer(IoCContainer.Configuration.GetConnectionString("DefaultConnection"));
				});

			// AddIdentity adds cookie based authentication
			// Adds scoped classes for things like UserManager, SignInManager and so on
			// NOTE: Automatically adds the validated user from a cookie to the HttpContext.User
			services.AddIdentity<MotivoUser, IdentityRole>()
				// Adds UserStore and RoleStore
				.AddEntityFrameworkStores<MotivoDbContext>()
				// Adds a provider that generates unique keys and hashes for things like
				// forgot password links, phone number verification
				.AddDefaultTokenProviders();

			services.AddAuthentication()
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = IoCContainer.Configuration["Jwt:JwtIssuer"],
						ValidAudience = IoCContainer.Configuration["Jwt:JwtAudience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IoCContainer.Configuration["Jwt:SecretKey"]))
					};
				});
			

			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			IoCContainer.Provider =  serviceProvider as ServiceProvider;

			app.UseAuthentication();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
