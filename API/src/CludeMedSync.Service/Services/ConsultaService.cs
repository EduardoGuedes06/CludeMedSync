using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Services
{
	public class ConsultaService : IConsultaService
	{
		private readonly IConsultaRepository _consultaRepository;
		private readonly IPacienteRepository _pacienteRepository;
		private readonly IProfissionalRepository _profissionalRepository;
		private readonly IConsultaLogRepository _consultaLogRepository;

		public ConsultaService(
			IConsultaRepository consultaRepository,
			IPacienteRepository pacienteRepository,
			IProfissionalRepository profissionalRepository,
			IConsultaLogRepository consultaLogRepository)
		{
			_consultaRepository = consultaRepository;
			_pacienteRepository = pacienteRepository;
			_profissionalRepository = profissionalRepository;
			_consultaLogRepository = consultaLogRepository;
		}

		public async Task<ResultadoOperacao<ConsultaDto>> AgendarAsync(AgendarConsultaDto dto, Guid idUsuario)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(dto.PacienteId);
			if(paciente is null)
				return ResultadoOperacao<ConsultaDto>.Falha("Paciente não encontrado", null, 404);

			var profissional = await _profissionalRepository.GetByIdAsync(dto.ProfissionalId);
				if (profissional is null)
					return ResultadoOperacao<ConsultaDto>.Falha("Profissional não encontrado.", null, 404);

			if (await _consultaRepository.ExisteConsultaNoMesmoDiaAsync(dto.PacienteId, dto.ProfissionalId, dto.DataHoraInicio))
				return ResultadoOperacao<ConsultaDto>.Falha("Este paciente já possui uma consulta com este profissional no mesmo dia.");

			if (await _consultaRepository.ExisteConsultaNoMesmoHorarioAsync(dto.ProfissionalId, dto.DataHoraInicio))
				return ResultadoOperacao<ConsultaDto>.Falha("O profissional não está disponível neste horário.");

			var novaConsulta = Consulta.Agendar(idUsuario, dto.PacienteId, dto.ProfissionalId, dto.DataHoraInicio, dto.Motivo);
			var novoId = await _consultaRepository.AddAsync(novaConsulta);


			await LogConsultaAsync(novoId, novaConsulta, paciente, profissional);

			var consultaDto = new ConsultaDto(
				novoId,
				paciente.Id,
				paciente.NomeCompleto,
				profissional.Id,
				profissional.NomeCompleto,
				novaConsulta.DataHoraInicio,
				novaConsulta.DataHoraFim,
				novaConsulta.Status
			);
			return ResultadoOperacao<ConsultaDto>.Ok("Consulta agendada com sucesso.", consultaDto);
		}

		public async Task<ResultadoOperacao<object>> CancelarAsync(int id, Guid idUsuario)
		{
			var consulta = await _consultaRepository.GetByIdAsync(id);
			if (consulta is null)
				return ResultadoOperacao<object>.Falha("Consulta não encontrada.");

			consulta.Cancelar();

			var sucesso = await _consultaRepository.UpdateAsync(consulta);
			if (!sucesso)
				return ResultadoOperacao<object>.Falha("Erro ao atualizar o status da consulta.");

			var paciente = await _pacienteRepository.GetByIdAsync(consulta.PacienteId);
			var profissional = await _profissionalRepository.GetByIdAsync(consulta.ProfissionalId);

			if (paciente != null && profissional != null)
			{
				await LogConsultaAsync(consulta.Id, consulta, paciente, profissional);
			}

			return ResultadoOperacao<object>.Ok("Consulta cancelada com sucesso.");
		}

		public async Task<IEnumerable<ConsultaDto>> GetAllAsync()
		{
			var consultas = await _consultaRepository.GetAllAsync();
			var dtos = new List<ConsultaDto>();
			foreach (var consulta in consultas)
			{
				var paciente = await _pacienteRepository.GetByIdAsync(consulta.PacienteId);
				var profissional = await _profissionalRepository.GetByIdAsync(consulta.ProfissionalId);

				dtos.Add(new ConsultaDto(
					consulta.Id,
					consulta.PacienteId,
					paciente?.NomeCompleto ?? "Paciente não encontrado",
					consulta.ProfissionalId,
					profissional?.NomeCompleto ?? "Profissional não encontrado",
					consulta.DataHoraInicio,
					consulta.DataHoraFim,
					consulta.Status
				));
			}
			return dtos;
		}

		public async Task<ConsultaDto?> GetByIdAsync(int id)
		{
			var consulta = await _consultaRepository.GetByIdAsync(id);
			if (consulta == null) return null;
			var paciente = await _pacienteRepository.GetByIdAsync(consulta.PacienteId);
			var profissional = await _profissionalRepository.GetByIdAsync(consulta.ProfissionalId);

			return new ConsultaDto(
				consulta.Id,
				consulta.PacienteId,
				paciente?.NomeCompleto ?? "Paciente não encontrado",
				consulta.ProfissionalId,
				profissional?.NomeCompleto ?? "Profissional não encontrado",
				consulta.DataHoraInicio,
				consulta.DataHoraFim,
				consulta.Status
			);
		}

		private async Task LogConsultaAsync(int consultaId, Consulta consulta, Paciente paciente, Profissional profissional)
		{
			var log = new ConsultaLog
			{
				UsuarioId = consulta.UsuarioId,
				ConsultaId = consultaId,
				PacienteId = paciente.Id,
				ProfissionalId = profissional.Id,
				NomePaciente = paciente.NomeCompleto,
				NomeProfissional = profissional.NomeCompleto,
				DataHoraInicio = consulta.DataHoraInicio,
				DataHoraFim = consulta.DataHoraFim,
				Motivo = consulta.Motivo,
				Status = consulta.Status,
				DataLog = DateTime.UtcNow
			};
			await _consultaLogRepository.AddAsync(log);
		}
	}
}
