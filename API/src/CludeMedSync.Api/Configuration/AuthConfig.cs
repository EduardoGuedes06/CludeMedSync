using CludeMedSync.Api.Extensions;
using CludeMedSync.Data.Auth;
using CludeMedSync.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CludeMedSync.Api.Configuration;

public static class AuthConfig
{
	public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentityCore<Usuario>(options =>
		{
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireUppercase = true;
			options.Password.RequiredLength = 8;
		})
		.AddUserStore<DapperUserStore>()
		.AddErrorDescriber<PortugueseIdentityErrorDescriber>()
		.AddDefaultTokenProviders();
		var appSettings = configuration.GetSection("AppSettings");
		var secretKey = appSettings["Secret"];
		if (string.IsNullOrEmpty(secretKey))
		{
			throw new InvalidOperationException("A chave secreta do JWT (AppSettings:Secret) não está configurada ou está vazia no appsettings.json.");
		}

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = true;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
				ValidateIssuer = true,
				ValidIssuer = appSettings["Issuer"],
				ValidateAudience = true,
				ValidAudience = appSettings["Audience"],
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
		});

		return services;
	}

	public static IApplicationBuilder UseAuthConfig(this IApplicationBuilder app)
	{
		app.UseAuthentication();
		app.UseAuthorization();
		return app;
	}
}
