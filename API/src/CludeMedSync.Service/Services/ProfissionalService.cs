using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Domain.Models.Utils.Enums;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Services
{
	public class ProfissionalService : IProfissionalService
	{
		private readonly IProfissionalRepository _profissionalRepository;
		private readonly IConsultaRepository _consultaRepository;

		public ProfissionalService(
			IProfissionalRepository profissionalRepository, 
			IConsultaRepository consultaRepository)
		{
			_profissionalRepository = profissionalRepository;
			_consultaRepository = consultaRepository;
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

		public async Task<ResultadoOperacao<ProfissionalDto>> CreateAsync(CreateProfissionalDto profissionalDto)
		{
			var (existe, mensagem) = await _profissionalRepository.VerificarDuplicidadeProfissionalAsync(
				profissionalDto.CRM,
				profissionalDto.Email,
				profissionalDto.Telefone
			);

			if (existe)
				return ResultadoOperacao<ProfissionalDto>.Falha(mensagem);

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

			var dto = new ProfissionalDto(
				profissional.Id,
				profissional.NomeCompleto,
				profissional.Especialidade,
				profissional.CRM,
				profissional.Email,
				profissional.Telefone
			);

			return ResultadoOperacao<ProfissionalDto>.Ok("Profissional cadastrado com sucesso.", dto);
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
		public async Task<ResultadoOperacao<object>> DeleteAsync(int id)
		{
			var profissional = await _profissionalRepository.GetByIdAsync(id);
			if (profissional == null)
				return ResultadoOperacao<object>.Falha("profissional não encontrado.");

			var consulta = await _consultaRepository.GetByRelationShip(
				nameof(Consulta.ProfissionalId),
				id.ToString(),
				EnumTipoAtributo.Inteiro
			);

			if (consulta != null)
				return ResultadoOperacao<object>.Falha("Não é possível excluir o Profissional. Consulta vinculada encontrada.", consulta);

			var sucesso = await _profissionalRepository.DeleteAsync(id);
			return sucesso
				? ResultadoOperacao<object>.Ok("Profissional excluído com sucesso.")
				: ResultadoOperacao<object>.Falha("Erro ao excluir paciente.");
		}
	}
}
