using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CludeMedSync.Api.Configuration.HealthChecks.Infra
{
	public class MemoryHealthCheck : IHealthCheck
	{
		public Task<HealthCheckResult> CheckHealthAsync(
			HealthCheckContext context,
			CancellationToken cancellationToken = default)
		{
			var allocated = GC.GetTotalMemory(forceFullCollection: false);
			var memoryThreshold = 1024L * 1024L * 500L;

			var data = new Dictionary<string, object>
		{
			{ "MemoriaAlocada", $"{allocated / 1024 / 1024} MB" },
			{ "Limite", $"{memoryThreshold / 1024 / 1024} MB" }
		};

			if (allocated < memoryThreshold)
			{
				return Task.FromResult(HealthCheckResult.Healthy(
					"O consumo de memória está dentro do limite aceitável.", data));
			}

			return Task.FromResult(HealthCheckResult.Unhealthy(
				"Atenção: o consumo de memória está alto.", data: data));
		}
	}
}
