using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CludeMedSync.Api.Configuration;

public static class SwaggerConfig
{
	public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "CludeMedSync API",
				Version = "v1",
				Description = "API para o sistema de agendamento de consultas CludeMedSync.",
				Contact = new OpenApiContact { Name = "Seu Nome", Email = "seu-email@provedor.com" },
				License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
			});

			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "Insira o token JWT desta maneira: Bearer {seu token}",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT"
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] {}
				}
			});

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);
		});

		return services;
	}

	public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI(c =>
		{
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "CludeMedSync API V1");
		});

		return app;
	}
}
