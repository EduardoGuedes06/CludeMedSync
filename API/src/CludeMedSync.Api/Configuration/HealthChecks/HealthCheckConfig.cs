using CludeMedSync.Api.Configuration.HealthChecks.Infra;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CludeMedSync.Api.Configuration
{
	public static class HealthCheckConfig
	{
		public static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHealthChecks()
				.AddMySql(
					configuration.GetConnectionString("connection"),
					name: "MySQL",
					failureStatus: HealthStatus.Unhealthy,
					tags: new[] { "database" })
				.AddCheck<MemoryHealthCheck>(
					"MemoriaApp",
					failureStatus: HealthStatus.Degraded,
					tags: new[] { "system" });


			services.AddHealthChecksUI()
				.AddInMemoryStorage();

			return services;
		}

		public static IApplicationBuilder UseHealthCheckConfig(this IApplicationBuilder app)
		{
			app.UseHealthChecks("/health/db", new HealthCheckOptions
			{
				Predicate = check => check.Tags.Contains("database"),
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});

			app.UseHealthChecks("/health/system", new HealthCheckOptions
			{
				Predicate = check => check.Tags.Contains("system"),
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});

			app.UseHealthChecksUI(options =>
			{
				options.UIPath = "/saude-ui";
			});

			return app;
			return app;
		}
	}
}