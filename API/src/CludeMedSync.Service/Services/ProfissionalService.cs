using AutoMapper;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Domain.Models.Utils.Enums;
using CludeMedSync.Models.Request;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.Interfaces;

namespace CludeMedSync.Service.Services
{
	public class ProfissionalService : BaseService<Profissional>, IProfissionalService
	{
		private readonly IProfissionalRepository _profissionalRepository;
		private readonly IConsultaRepository _consultaRepository;

		public ProfissionalService(
			IProfissionalRepository profissionalRepository,
			IConsultaRepository consultaRepository,
			IMapper mapper,
			IPagedResultRepository<Profissional> pagedRepository)
			: base(mapper, pagedRepository)
		{
			_profissionalRepository = profissionalRepository;
			_consultaRepository = consultaRepository;
		}

		public async Task<ProfissionalResponse?> GetByIdAsync(int id)
		{
			var profissional = await _profissionalRepository.GetByIdAsync(id);
			return _mapper.Map<ProfissionalResponse>(profissional);
		}

		public async Task<IEnumerable<ProfissionalResponse>> GetAllAsync()
		{
			var profissionais = await _profissionalRepository.GetAllAsync();

			return _mapper.Map<IEnumerable<ProfissionalResponse>>(profissionais);

		}

		public async Task<ResultadoOperacao<ProfissionalResponse>> CreateAsync(ProfissionalRequest profissionalRequest)
		{
			var (existe, mensagem) = await _profissionalRepository.VerificarDuplicidadeProfissionalAsync(
				profissionalRequest.CRM,
				profissionalRequest.Email,
				profissionalRequest.Telefone
			);

			if (existe)
				return ResultadoOperacao<ProfissionalResponse>.Falha(mensagem);

			var profissional = _mapper.Map<Profissional>(profissionalRequest);

			var novoId = await _profissionalRepository.AddAsync(profissional);
			profissional.Id = novoId;

			var dto = _mapper.Map<ProfissionalResponse>(profissional);

			return ResultadoOperacao<ProfissionalResponse>.Ok("Profissional cadastrado com sucesso.", dto);
		}

		public async Task<bool> UpdateAsync(int id, ProfissionalRequest ProfissionalResponse)
		{
			var profissional = await _profissionalRepository.GetByIdAsync(id);
			if (profissional == null) return false;

			profissional.NomeCompleto = ProfissionalResponse.NomeCompleto;
			profissional.Especialidade = ProfissionalResponse.Especialidade;
			profissional.CRM = ProfissionalResponse.CRM;
			profissional.Email = ProfissionalResponse.Email;
			profissional.Telefone = ProfissionalResponse.Telefone;

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
