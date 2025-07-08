using CludeMedSync.Domain.Entities.Pagination;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Domain.Models;
using CludeMedSync.Domain.Models.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Interfaces
{
	public interface IConsultaRepository : IRepository<Consulta>
	{
		Task<bool> ExisteConsultaNoMesmoDiaAsync(int pacienteId, int profissionalId, DateTime data);
		Task<bool> ExisteConsultaNoMesmoHorarioAsync(int profissionalId, DateTime dataHoraInicio);
		Task<(Consulta? consulta, Paciente? paciente, Profissional? profissional)> GetByIdComAgregadosAsync(int id);
		Task<Consulta> GetByRelationShip(string coluna, string valor, EnumTipoAtributo tipo);
		Task<PagedResult<ConsultaCompleta>> GetConsultasPaginadoAsync(int page, int pageSize, object? filtros = null, string? orderBy = null, bool orderByDesc = false);
	}
}
