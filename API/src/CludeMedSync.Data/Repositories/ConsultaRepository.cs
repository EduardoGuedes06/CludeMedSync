using CludeMedSync.Data.Context;
using CludeMedSync.Data.Repositories.Utils;
using CludeMedSync.Domain.Entities.Pagination;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Domain.Models.Utils.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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

		public async Task<PagedResult<ConsultaCompleta>> GetConsultasPaginadoAsync(
			int page, int pageSize, object? filtros = null, string? orderBy = null, bool orderByDesc = false)
		{
			string colunas = @$"
			co.Id AS {nameof(ConsultaCompleta.ConsultaId)},
			co.UsuarioId AS {nameof(ConsultaCompleta.UsuarioId)},
			co.PacienteId AS {nameof(ConsultaCompleta.PacienteId)},
			COALESCE(p.NomeCompleto, 'Fora do Sistema') AS {nameof(ConsultaCompleta.PacienteNome)},
			co.ProfissionalId AS {nameof(ConsultaCompleta.ProfissionalId)},
			COALESCE(pr.NomeCompleto, 'Fora do Sistema') AS {nameof(ConsultaCompleta.ProfissionalNome)},
			co.DataHoraInicio AS {nameof(ConsultaCompleta.DataHoraInicio)},
			co.DataHoraFim AS {nameof(ConsultaCompleta.DataHoraFim)},
			CAST(co.Status AS CHAR) AS {nameof(ConsultaCompleta.Status)},
			co.Observacao AS {nameof(ConsultaCompleta.Observacao)}";
			string tabelas = @$"
			{nameof(Consulta)} co
			INNER JOIN {nameof(Paciente)} p ON co.PacienteId = p.Id
			INNER JOIN {nameof(Profissional)} pr ON co.ProfissionalId = pr.Id";

			return await GetPaginadoComplexoAsync<ConsultaCompleta>(
				page, pageSize, colunas, tabelas,
				filtrosDinamicos: filtros, orderBy: orderBy, orderByDesc: orderByDesc);
		}


		public async Task<(Consulta? consulta, Paciente? paciente, Profissional? profissional)> GetByIdComAgregadosAsync(int id)
		{
			using var connection = CreateConnection();
			const string query = @$"SELECT 
									C.*, 
									P.*, 
									PR.*
								FROM {nameof(Consulta)} AS C
								LEFT JOIN {nameof(Paciente)} AS P ON C.{nameof(Paciente)}Id = P.Id
								LEFT JOIN {nameof(Profissional)} AS PR ON C.{nameof(Profissional)}Id = PR.Id
								WHERE C.Id = @Id";

			var resultados = new List<(Consulta, Paciente, Profissional)>();

			await connection.QueryAsync<Consulta, Paciente, Profissional, Consulta>(
				query,
				(consulta, paciente, profissional) =>
				{
					resultados.Add((consulta, paciente, profissional));
					return consulta;
				},
				new { Id = id },
				splitOn: "Id,Id"
			);

			if (resultados.Any())
			{
				return resultados.First();
			}

			return (null, null, null);
		}

		public async Task<Consulta> GetByRelationShip(string coluna, string valor, EnumTipoAtributo tipo)
		{
			using var connection = CreateConnection();
			var query = $"SELECT * FROM {nameof(Consulta)} WHERE {coluna} <> @Valor";
			object valorConvertido;

			try
			{
				valorConvertido = tipo switch
				{
					EnumTipoAtributo.Inteiro => int.TryParse(valor, out var i) ? i : throw new FormatException("Valor inválido para inteiro."),
					EnumTipoAtributo.Decimal => decimal.TryParse(valor, out var d) ? d : throw new FormatException("Valor inválido para decimal."),
					EnumTipoAtributo.Booleano => valor.ToLower() switch
					{
						"true" or "1" => true,
						"false" or "0" => false,
						_ => throw new FormatException("Valor inválido para booleano.")
					},
					EnumTipoAtributo.Data or EnumTipoAtributo.DataHora => DateTime.TryParse(valor, out var dt) ? dt : throw new FormatException("Valor inválido para data."),
					EnumTipoAtributo.Guid => Guid.TryParse(valor, out var guid) ? guid : throw new FormatException("Valor inválido para GUID."),
					EnumTipoAtributo.Texto or EnumTipoAtributo.Enumerador => valor,
					_ => throw new NotSupportedException("Tipo de atributo não suportado.")
				};
			}
			catch (Exception ex)
			{
				throw new NotSupportedException(ex.Message);
			}

			var resultado = await connection.QueryFirstOrDefaultAsync<Consulta>(query, new { Valor = valorConvertido });
			return resultado;
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
