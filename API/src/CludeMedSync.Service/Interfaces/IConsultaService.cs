using CludeMedSync.Service.DTOs;

namespace CludeMedSync.Service.Interfaces
{
	public interface IConsultaService
	{
		Task<ConsultaDto?> GetByIdAsync(int id);
		Task<IEnumerable<ConsultaDto>> GetAllAsync();
		Task<ConsultaDto> AgendarAsync(AgendarConsultaDto consultaDto);
		Task<bool> CancelarAsync(int id);
	}
}
