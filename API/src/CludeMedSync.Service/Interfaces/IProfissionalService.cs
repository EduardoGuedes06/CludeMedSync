using CludeMedSync.Models.Request;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;

namespace CludeMedSync.Service.Interfaces
{
	public interface IProfissionalService
	{
		Task<object> ObterPaginadoGenericoAsync(
					int page,
					int pageSize,
					object? filtros = null,
					string? orderBy = null,
					bool orderByDesc = false,
					bool ativo = true,
					Type tipoDto = null!);
		Task<ProfissionalResponse?> GetByIdAsync(int id);
		Task<IEnumerable<ProfissionalResponse>> GetAllAsync();
		Task<bool> UpdateAsync(int id, ProfissionalRequest Profissional);
		Task<ResultadoOperacao<object>> DeleteAsync(int id);
		Task<ResultadoOperacao<ProfissionalResponse>> CreateAsync(ProfissionalRequest Profissional);
	}
}
