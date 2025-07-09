using CludeMedSync.Data.Context;
using CludeMedSync.Data.Repositories.Utils;
using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Entities.Pagination;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using Dapper;

public class ConsultaLogRepository : Repository<ConsultaLog>, IConsultaLogRepository
{
	public ConsultaLogRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
	{

	}

    public async Task<PagedResult<ConsultaLogCompleta>> GetConsultasLogPaginadoAsync(
        int page, int pageSize, object? filtros = null, string? orderBy = null, bool orderByDesc = false)
    {

		string colunas = @$"
            co.Id AS {nameof(ConsultaLogCompleta.LogId)},
            co.ConsultaId AS {nameof(ConsultaLogCompleta.ConsultaId)},
            co.UsuarioId AS {nameof(ConsultaLogCompleta.UsuarioId)},
            co.PacienteId AS {nameof(ConsultaLogCompleta.PacienteId)},
            COALESCE(p.NomeCompleto, 'Fora do Sistema') AS {nameof(ConsultaLogCompleta.PacienteNome)},
            co.ProfissionalId AS {nameof(ConsultaLogCompleta.ProfissionalId)},
            COALESCE(pr.NomeCompleto, 'Fora do Sistema') AS {nameof(ConsultaLogCompleta.ProfissionalNome)},
            co.DataHoraInicio AS {nameof(ConsultaLogCompleta.DataHoraInicio)},
            co.DataHoraFim AS {nameof(ConsultaLogCompleta.DataHoraFim)},
            co.Status AS {nameof(ConsultaLogCompleta.Status)},
            co.Observacao AS {nameof(ConsultaLogCompleta.Observacao)}";

		string tabelas = @$"
            {nameof(ConsultaLog)} co
            INNER JOIN {nameof(Paciente)} p ON co.PacienteId = p.Id
            INNER JOIN {nameof(Profissional)} pr ON co.ProfissionalId = pr.Id";


		return await GetPaginadoComplexoAsync<ConsultaLogCompleta>(
            page, pageSize, colunas, tabelas, 
            filtrosDinamicos: filtros, orderBy: orderBy, orderByDesc: orderByDesc);
    }
}
