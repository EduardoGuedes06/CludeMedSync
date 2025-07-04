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

		public PacienteService(
			IPacienteRepository pacienteRepository, 
			IConsultaRepository consultaRepository)
		{
			_pacienteRepository = pacienteRepository;
			_consultaRepository = consultaRepository;
		}

		public async Task<PacienteDto?> GetByIdAsync(int id)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			if (paciente == null) return null;
			return new PacienteDto(paciente.Id, paciente.NomeCompleto, paciente.DataNascimento, paciente.CPF, paciente.Email, paciente.Telefone);
		}

		public async Task<IEnumerable<PacienteDto>> GetAllAsync()
		{
			var pacientes = await _pacienteRepository.GetAllAsync();
			return pacientes.Select(p => new PacienteDto(p.Id, p.NomeCompleto, p.DataNascimento, p.CPF, p.Email, p.Telefone));
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

			var paciente = new Paciente
			{
				NomeCompleto = pacienteDto.NomeCompleto,
				DataNascimento = pacienteDto.DataNascimento,
				CPF = pacienteDto.CPF,
				Email = pacienteDto.Email,
				Telefone = pacienteDto.Telefone,
				Ativo = true
			};

			var novoId = await _pacienteRepository.AddAsync(paciente);
			paciente.Id = novoId;

			var dto = new PacienteDto(
				paciente.Id,
				paciente.NomeCompleto,
				paciente.DataNascimento,
				paciente.CPF,
				paciente.Email,
				paciente.Telefone
			);

			return ResultadoOperacao<PacienteDto>.Ok("Paciente cadastrado com sucesso.", dto);
		}


		public async Task<bool> UpdateAsync(int id, CreatePacienteDto pacienteDto)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			if (paciente == null) return false;

			paciente.NomeCompleto = pacienteDto.NomeCompleto;
			paciente.DataNascimento = pacienteDto.DataNascimento;
			paciente.CPF = pacienteDto.CPF;
			paciente.Email = pacienteDto.Email;
			paciente.Telefone = pacienteDto.Telefone;

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
