using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Data.Repositories
{
	public class PacienteRepository : Repository<Paciente>, IPacienteRepository
	{
		public PacienteRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		public async Task<Paciente?> GetByCpfAsync(string cpf)
		{
			using var connection = CreateConnection();
			var query = $"SELECT * FROM {nameof(Paciente)} WHERE CPF = @Cpf AND Ativo = 1";
			return await connection.QuerySingleOrDefaultAsync<Paciente>(query, new { Cpf = cpf });
		}
	}
}
