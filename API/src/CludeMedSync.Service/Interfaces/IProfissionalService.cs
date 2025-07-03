using CludeMedSync.Service.DTOs;

namespace CludeMedSync.Service.Interfaces
{
	public interface IProfissionalService
	{
		Task<ProfissionalDto?> GetByIdAsync(int id);
		Task<IEnumerable<ProfissionalDto>> GetAllAsync();
		Task<ProfissionalDto> CreateAsync(CreateProfissionalDto profissionalDto);
		Task<bool> UpdateAsync(int id, CreateProfissionalDto profissionalDto);
		Task<bool> DeleteAsync(int id);
	}
}
