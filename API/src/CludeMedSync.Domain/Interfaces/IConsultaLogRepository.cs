using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Entities.Pagination;
using CludeMedSync.Domain.Entities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Interfaces
{
	public interface IConsultaLogRepository : IRepository<ConsultaLog>
	{
		Task<PagedResult<ConsultaLogCompleta>> GetConsultasLogPaginadoAsync(int page, int pageSize, object? filtros = null, string? orderBy = null, bool orderByDesc = false);
	}
}
