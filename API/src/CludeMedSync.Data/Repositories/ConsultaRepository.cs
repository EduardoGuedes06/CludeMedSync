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
	public class ConsultaRepository : Repository<Consulta>, IConsultaRepository
	{
		public ConsultaRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		public override async Task<IEnumerable<Consulta>> GetAllAsync()
		{
			using var connection = CreateConnection();
			const string query = $"SELECT * FROM {nameof(Consulta)} WHERE Status <> 'Cancelada'";
			return await connection.QueryAsync<Consulta>(query);
		}

		public override async Task<bool> DeleteAsync(int id)
		{
			using var connection = CreateConnection();
			const string query = $"UPDATE {nameof(Consulta)} SET Status = 'Cancelada' WHERE Id = @Id";
			var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
			return affectedRows > 0;
		}

		public async Task<bool> ExisteConsultaNoMesmoDiaAsync(int pacienteId, int profissionalId, DateTime data)
		{
			using var connection = CreateConnection();
			const string query = @$"
            SELECT 1 FROM {nameof(Consulta)} 
            WHERE PacienteId = @PacienteId 
              AND ProfissionalId = @ProfissionalId 
              AND DATE(DataHoraInicio) = DATE(@Data)
              AND Status <> 'Cancelada'
            LIMIT 1";

			var result = await connection.ExecuteScalarAsync<int?>(query, new { pacienteId, profissionalId, data });
			return result.HasValue;
		}

		public async Task<bool> ExisteConsultaNoMesmoHorarioAsync(int profissionalId, DateTime dataHoraInicio)
		{
			using var connection = CreateConnection();
			var dataHoraFim = dataHoraInicio.AddMinutes(30);

			const string query = @$"
            SELECT 1 FROM {nameof(Consulta)}
            WHERE ProfissionalId = @ProfissionalId
              AND Status <> 'Cancelada'
              AND @DataHoraInicio < DataHoraFim
              AND @DataHoraFim > DataHoraInicio
            LIMIT 1";

			var result = await connection.ExecuteScalarAsync<int?>(query, new { profissionalId, dataHoraInicio, dataHoraFim });
			return result.HasValue;
		}
	}
}
