using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;

namespace CludeMedSync.Service.Services
{
	public class ProfissionalService : IProfissionalService
	{
		private readonly IProfissionalRepository _profissionalRepository;

		public ProfissionalService(IProfissionalRepository profissionalRepository)
		{
			_profissionalRepository = profissionalRepository;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var profissional = await _profissionalRepository.GetByIdAsync(id);
			if (profissional == null) return false;

			return await _profissionalRepository.DeleteAsync(id);
		}

		public async Task<ProfissionalDto?> GetByIdAsync(int id)
		{
			var profissional = await _profissionalRepository.GetByIdAsync(id);
			if (profissional == null) return null;

			return new ProfissionalDto(
				profissional.Id,
				profissional.NomeCompleto,
				profissional.Especialidade,
				profissional.CRM,
				profissional.Email,
				profissional.Telefone
			);
		}

		public async Task<IEnumerable<ProfissionalDto>> GetAllAsync()
		{
			var profissionais = await _profissionalRepository.GetAllAsync();
			return profissionais.Select(p => new ProfissionalDto(
				p.Id,
				p.NomeCompleto,
				p.Especialidade,
				p.CRM,
				p.Email,
				p.Telefone
			));
		}

		public async Task<ProfissionalDto> CreateAsync(CreateProfissionalDto profissionalDto)
		{
			var profissionalExistente = await _profissionalRepository.GetByCrmAsync(profissionalDto.CRM);
			if (profissionalExistente != null)
			{
				throw new InvalidOperationException("Já existe um profissional cadastrado com este CRM.");
			}

			var profissional = new Profissional
			{
				NomeCompleto = profissionalDto.NomeCompleto,
				Especialidade = profissionalDto.Especialidade,
				CRM = profissionalDto.CRM,
				Email = profissionalDto.Email,
				Telefone = profissionalDto.Telefone
			};

			var novoId = await _profissionalRepository.AddAsync(profissional);
			profissional.Id = novoId;

			return new ProfissionalDto(profissional.Id, profissional.NomeCompleto, profissional.Especialidade, profissional.CRM, profissional.Email, profissional.Telefone);
		}

		public async Task<bool> UpdateAsync(int id, CreateProfissionalDto profissionalDto)
		{
			var profissional = await _profissionalRepository.GetByIdAsync(id);
			if (profissional == null) return false;

			profissional.NomeCompleto = profissionalDto.NomeCompleto;
			profissional.Especialidade = profissionalDto.Especialidade;
			profissional.CRM = profissionalDto.CRM;
			profissional.Email = profissionalDto.Email;
			profissional.Telefone = profissionalDto.Telefone;

			return await _profissionalRepository.UpdateAsync(profissional);
		}
	}
}
