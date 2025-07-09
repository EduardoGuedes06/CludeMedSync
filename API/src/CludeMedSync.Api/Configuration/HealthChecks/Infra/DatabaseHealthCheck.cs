using CludeMedSync.Data.Context;
using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CludeMedSync.Api.Configuration.HealthChecks.Infra
{
	public class DatabaseHealthCheck : IHealthCheck
	{
		private readonly DbConnectionFactory _connectionFactory;

		public DatabaseHealthCheck(DbConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}
		public async Task<HealthCheckResult> CheckHealthAsync(
			HealthCheckContext context,
			CancellationToken cancellationToken = default)
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				await connection.ExecuteAsync("SELECT 1");
				return HealthCheckResult.Healthy("A conexão com o banco de dados MySQL está funcionando.");
			}
			catch (Exception ex)
			{
				return HealthCheckResult.Unhealthy("Não foi possível conectar ao banco de dados MySQL.", exception: ex);
			}
		}
	}
}
