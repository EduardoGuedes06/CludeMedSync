
using CludeMedSync.Services.DTOs;

namespace CludeMedSync.Domain.Interfaces
{
	public interface IPacienteService
	{
		Task<PacienteDto?> GetByIdAsync(int id);
		Task<IEnumerable<PacienteDto>> GetAllAsync();
		Task<PacienteDto> CreateAsync(CreatePacienteDto pacienteDto);
		Task<bool> UpdateAsync(int id, CreatePacienteDto pacienteDto);
		Task<bool> DeleteAsync(int id);
	}
}
