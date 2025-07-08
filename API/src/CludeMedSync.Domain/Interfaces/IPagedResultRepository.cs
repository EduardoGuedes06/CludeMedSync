using CludeMedSync.Domain.Entities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Interfaces
{
	public interface IPagedResultRepository<T> where T : class
	{
		Task<PagedResult<T>> ObterPaginadoGenericoAsync(int page, int pageSize, object? filtros = null, string? orderBy = null, bool? ativo = null, bool orderByDesc = false);
	}
}
