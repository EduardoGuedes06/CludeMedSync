using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.Models.Request;
using CludeMedSync.Service.Models.Response;

namespace CludeMedSync.Service.Interfaces
{
	public interface IConsultaService
	{
		Task<ResultadoOperacao<ConsultaResponse>> AgendarAsync(AgendarConsultaRequest dto, Guid usuarioId);
		Task<IEnumerable<ConsultaResponse>> GetAllAsync();
		Task<ConsultaResponse?> GetByIdAsync(int id);
		Task<ResultadoOperacao<object>> ConfirmarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> CancelarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> IniciarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> FinalizarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> MarcarComoPacienteNaoCompareceuAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> MarcarComoProfissionalNaoCompareceuAsync(int id, Guid usuarioId);
		Task<IEnumerable<ConsultaLogResponse>> GetAlllogsAsync();
		Task<object> ObterConsultasPaginadoAsync(int page, int pageSize, object? filtros = null, string? orderBy = null, bool orderByDesc = false);
		Task<object> ObterConsultasLogPaginadoAsync(int page, int pageSize, object? filtros = null, string? orderBy = null, bool orderByDesc = false);
	}
}
