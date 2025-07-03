using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using Dapper;

namespace CludeMedSync.Data.Repositories
{
	public class ProfissionalRepository : Repository<Profissional>, IProfissionalRepository
	{
		public ProfissionalRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		public async Task<Profissional?> GetByCrmAsync(string crm)
		{
			using var connection = CreateConnection();
			const string query = $"SELECT * FROM {nameof(Profissional)} WHERE CRM = @CRM AND Ativo = 1";
			return await connection.QuerySingleOrDefaultAsync<Profissional>(query, new { CRM = crm });
		}
	}
}
