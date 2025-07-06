using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;

namespace CludeMedSync.Service.Interfaces
{
	public interface IConsultaService
	{
		Task<ResultadoOperacao<ConsultaDto>> AgendarAsync(AgendarConsultaDto dto, Guid usuarioId);
		Task<IEnumerable<ConsultaDto>> GetAllAsync();
		Task<ConsultaDto?> GetByIdAsync(int id);
		Task<ResultadoOperacao<object>> ConfirmarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> CancelarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> IniciarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> FinalizarAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> MarcarComoPacienteNaoCompareceuAsync(int id, Guid usuarioId);
		Task<ResultadoOperacao<object>> MarcarComoProfissionalNaoCompareceuAsync(int id, Guid usuarioId);
	}
}
