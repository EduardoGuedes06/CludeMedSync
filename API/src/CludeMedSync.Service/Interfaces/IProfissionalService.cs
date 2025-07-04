using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;

namespace CludeMedSync.Service.Interfaces
{
	public interface IProfissionalService
	{
		Task<ProfissionalDto?> GetByIdAsync(int id);
		Task<IEnumerable<ProfissionalDto>> GetAllAsync();
		Task<bool> UpdateAsync(int id, CreateProfissionalDto profissionalDto);
		Task<ResultadoOperacao<object>> DeleteAsync(int id);
		Task<ResultadoOperacao<ProfissionalDto>> CreateAsync(CreateProfissionalDto profissionalDto);
	}
}
