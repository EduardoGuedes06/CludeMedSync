
using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;

namespace CludeMedSync.Service.Interfaces
{
	public interface IPacienteService
	{
		Task<PacienteDto?> GetByIdAsync(int id);
		Task<IEnumerable<PacienteDto>> GetAllAsync();
		Task<bool> UpdateAsync(int id, CreatePacienteDto pacienteDto);
		Task<ResultadoOperacao<object>> DeleteAsync(int id);
		Task<ResultadoOperacao<PacienteDto>> CreateAsync(CreatePacienteDto pacienteDto);
	}
}
