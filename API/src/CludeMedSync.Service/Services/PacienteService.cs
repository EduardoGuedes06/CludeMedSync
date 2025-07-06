using AutoMapper;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Domain.Models.Utils.Enums;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;

namespace CludeMedSync.Service.Services
{
	public class PacienteService : IPacienteService
	{
		private readonly IPacienteRepository _pacienteRepository;
		private readonly IConsultaRepository _consultaRepository;
		private readonly IMapper _mapper;

		public PacienteService(
			IPacienteRepository pacienteRepository, 
			IConsultaRepository consultaRepository,
			IMapper mapper)
		{
			_pacienteRepository = pacienteRepository;
			_consultaRepository = consultaRepository;
			_mapper = mapper;
		}

		public async Task<PacienteDto?> GetByIdAsync(int id)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			return _mapper.Map<PacienteDto>(paciente);
		}

		public async Task<IEnumerable<PacienteDto>> GetAllAsync()
		{
			var pacientes = await _pacienteRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<PacienteDto>>(pacientes);
		}

		public async Task<ResultadoOperacao<PacienteDto>> CreateAsync(CreatePacienteDto pacienteDto)
		{
			var (existe, mensagem) = await _pacienteRepository.VerificarDuplicidadePacienteAsync(
				pacienteDto.CPF,
				pacienteDto.Email,
				pacienteDto.Telefone
			);

			if (existe)
				return ResultadoOperacao<PacienteDto>.Falha(mensagem);

			var paciente = _mapper.Map<Paciente>(pacienteDto);
			paciente.Ativo = true;

			var novoId = await _pacienteRepository.AddAsync(paciente);
			paciente.Id = novoId;

			var dto = _mapper.Map<PacienteDto>(paciente);

			return ResultadoOperacao<PacienteDto>.Ok("Paciente cadastrado com sucesso.", dto);

		}

		public async Task<bool> UpdateAsync(int id, CreatePacienteDto pacienteDto)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			if (paciente == null) return false;

			_mapper.Map(pacienteDto, paciente);

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
