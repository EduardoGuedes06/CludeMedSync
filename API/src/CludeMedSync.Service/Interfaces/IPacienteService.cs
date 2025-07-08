
using CludeMedSync.Models.Request;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;

namespace CludeMedSync.Service.Interfaces
{
	public interface IPacienteService
	{
		Task<object> ObterPaginadoGenericoAsync(
					int page,
					int pageSize,
					object? filtros = null,
					string? orderBy = null,
					bool orderByDesc = false,
					bool ativo = true,
					Type tipoDto = null!); 
		Task<PacienteResponse?> GetByIdAsync(int id);
		Task<IEnumerable<PacienteResponse>> GetAllAsync();
		Task<bool> UpdateAsync(int id, PacienteRequest PacienteResponse);
		Task<ResultadoOperacao<object>> DeleteAsync(int id);
		Task<ResultadoOperacao<PacienteResponse>> CreateAsync(PacienteRequest PacienteResponse);
	}
}
