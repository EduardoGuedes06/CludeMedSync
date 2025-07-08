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
	public class PacienteService : BaseService<Paciente>, IPacienteService
	{
		private readonly IPacienteRepository _pacienteRepository;
		private readonly IConsultaRepository _consultaRepository;

		public PacienteService(
			IPacienteRepository pacienteRepository,
			IConsultaRepository consultaRepository,
			IMapper mapper,
			IPagedResultRepository<Paciente> pagedRepository)
			: base(mapper, pagedRepository)
		{
			_pacienteRepository = pacienteRepository;
			_consultaRepository = consultaRepository;
		}

		public async Task<PacienteResponse?> GetByIdAsync(int id)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			return _mapper.Map<PacienteResponse>(paciente);
		}

		public async Task<IEnumerable<PacienteResponse>> GetAllAsync()
		{
			var pacientes = await _pacienteRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<PacienteResponse>>(pacientes);
		}

		public async Task<ResultadoOperacao<PacienteResponse>> CreateAsync(PacienteRequest PacienteResponse)
		{
			var (existe, mensagem) = await _pacienteRepository.VerificarDuplicidadePacienteAsync(
				PacienteResponse.CPF,
				PacienteResponse.Email,
				PacienteResponse.Telefone
			);

			if (existe)
				return ResultadoOperacao<PacienteResponse>.Falha(mensagem);

			var paciente = _mapper.Map<Paciente>(PacienteResponse);
			paciente.Ativo = true;

			var novoId = await _pacienteRepository.AddAsync(paciente);
			paciente.Id = novoId;

			var dto = _mapper.Map<PacienteResponse>(paciente);

			return ResultadoOperacao<PacienteResponse>.Ok("Paciente cadastrado com sucesso.", dto);

		}

		public async Task<bool> UpdateAsync(int id, PacienteRequest PacienteResponse)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			if (paciente == null) return false;

			_mapper.Map(PacienteResponse, paciente);

			return await _pacienteRepository.UpdateAsync(paciente);
		}

		public async Task<ResultadoOperacao<object>> DeleteAsync(int id)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			if (paciente == null)
				return ResultadoOperacao<object>.Falha("Paciente não encontrado.");

			var consulta = await _consultaRepository.GetByRelationShip(
				nameof(Consulta.PacienteId),
				id.ToString(),
				EnumTipoAtributo.Inteiro
			);

			if (consulta != null)
				return ResultadoOperacao<object>.Falha("Não é possível excluir o paciente. Consulta vinculada encontrada.", consulta);

			var sucesso = await _pacienteRepository.DeleteAsync(id);
			return sucesso
				? ResultadoOperacao<object>.Ok("Paciente excluído com sucesso.")
				: ResultadoOperacao<object>.Falha("Erro ao excluir paciente.");
		}
	}
}
