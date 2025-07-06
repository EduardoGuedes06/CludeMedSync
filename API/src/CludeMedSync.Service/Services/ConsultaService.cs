using AutoMapper;
using CludeMedSync.Domain.Entities;
using CludeMedSync.Domain.Entities.Utils.Enums;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;

namespace CludeMedSync.Service.Services
{
	public class ConsultaService : IConsultaService
	{
		private readonly IConsultaRepository _consultaRepository;
		private readonly IPacienteRepository _pacienteRepository;
		private readonly IProfissionalRepository _profissionalRepository;
		private readonly IConsultaLogRepository _consultaLogRepository;
		private readonly IMapper _mapper;

		public ConsultaService(
			IConsultaRepository consultaRepository,
			IPacienteRepository pacienteRepository,
			IProfissionalRepository profissionalRepository,
			IConsultaLogRepository consultaLogRepository,
			IMapper mapper)
		{
			_consultaRepository = consultaRepository;
			_pacienteRepository = pacienteRepository;
			_profissionalRepository = profissionalRepository;
			_consultaLogRepository = consultaLogRepository;
			_mapper = mapper;
		}

		#region Consulta (CRUD e Atualização Manual)

		public async Task<IEnumerable<ConsultaDto>> GetAllAsync()
		{
			var consultas = await _consultaRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ConsultaDto>>(consultas);
		}

		public async Task<ConsultaDto?> GetByIdAsync(int id)
		{
			var (consulta, paciente, profissional) = await _consultaRepository.GetByIdComAgregadosAsync(id);
			if (consulta == null || paciente == null || profissional == null)
			{
				return null;
			}

			return _mapper.Map<ConsultaDto>(consulta);
		}

		public async Task<IEnumerable<ConsultaLogDto>> GetLogsAsync()
		{
			var logs = await _consultaLogRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ConsultaLogDto>>(logs);
		}

		public async Task<ResultadoOperacao<ConsultaDto>> AgendarAsync(AgendarConsultaDto dto, Guid usuarioId)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(dto.PacienteId);
			if (paciente is null)
				return ResultadoOperacao<ConsultaDto>.Falha("Paciente não encontrado", status: 404);

			var profissional = await _profissionalRepository.GetByIdAsync(dto.ProfissionalId);
			if (profissional is null)
				return ResultadoOperacao<ConsultaDto>.Falha("Profissional não encontrado.", status: 404);

			if (await _consultaRepository.ExisteConsultaNoMesmoDiaAsync(dto.PacienteId, dto.ProfissionalId, dto.DataHoraInicio))
				return ResultadoOperacao<ConsultaDto>.Falha("Este paciente já possui uma consulta com este profissional no mesmo dia.");

			if (await _consultaRepository.ExisteConsultaNoMesmoHorarioAsync(dto.ProfissionalId, dto.DataHoraInicio))
				return ResultadoOperacao<ConsultaDto>.Falha("O profissional não está disponível neste horário.");

			var novaConsulta = Consulta.Agendar(usuarioId, dto.PacienteId, dto.ProfissionalId, dto.DataHoraInicio, dto.Motivo, dto.Observacao);
			var novoId = await _consultaRepository.AddAsync(novaConsulta);

			await LogConsultaAsync(novoId, novaConsulta, paciente, profissional);

			var consultaDto = _mapper.Map<ConsultaDto>(novaConsulta);
			return ResultadoOperacao<ConsultaDto>.Ok("Consulta agendada com sucesso.", consultaDto);
		}

		public async Task<ResultadoOperacao<ConsultaDto>> AtualizarAsync(int id, AtualizarConsultaDto dto, Guid usuarioId)
		{
			var (consulta, paciente, profissional) = await _consultaRepository.GetByIdComAgregadosAsync(id);

			if (consulta is null)
				return ResultadoOperacao<ConsultaDto>.Falha("Consulta não encontrada.", status: 404);

			if (consulta.UsuarioId != usuarioId)
				return ResultadoOperacao<ConsultaDto>.Falha("Você não tem permissão para alterar esta consulta.", status: 403);

			try
			{
				consulta.AtualizarDados(dto.DataHoraInicio, dto.Motivo, dto.Observacao);
			}
			catch (Exception ex)
			{
				return ResultadoOperacao<ConsultaDto>.Falha(ex.Message);
			}

			var sucesso = await _consultaRepository.UpdateAsync(consulta);
			if (!sucesso)
				return ResultadoOperacao<ConsultaDto>.Falha("Erro ao atualizar a consulta.");

			await LogConsultaAsync(consulta.Id, consulta, paciente!, profissional!);

			var consultaDto = _mapper.Map<ConsultaDto>(consulta);
			return ResultadoOperacao<ConsultaDto>.Ok("Consulta atualizada com sucesso.", consultaDto);
		}

		#endregion

		#region Transições de Estado da Consulta

		public Task<ResultadoOperacao<object>> ConfirmarAsync(int id, Guid usuarioId) => AlterarStatusConsultaAsync(id, usuarioId, c => c.Confirmar());
		public Task<ResultadoOperacao<object>> IniciarAsync(int id, Guid usuarioId) => AlterarStatusConsultaAsync(id, usuarioId, c => c.Iniciar());
		public Task<ResultadoOperacao<object>> FinalizarAsync(int id, Guid usuarioId) => AlterarStatusConsultaAsync(id, usuarioId, c => c.Finalizar());
		public Task<ResultadoOperacao<object>> CancelarAsync(int id, Guid usuarioId) => AlterarStatusConsultaAsync(id, usuarioId, c => c.Cancelar());
		public Task<ResultadoOperacao<object>> MarcarComoPacienteNaoCompareceuAsync(int id, Guid usuarioId) => AlterarStatusConsultaAsync(id, usuarioId, c => c.MarcarComoPacienteNaoCompareceu());
		public Task<ResultadoOperacao<object>> MarcarComoProfissionalNaoCompareceuAsync(int id, Guid usuarioId) => AlterarStatusConsultaAsync(id, usuarioId, c => c.MarcarComoProfissionalNaoCompareceu());

		#endregion

		#region Métodos Auxiliares

		private async Task<ResultadoOperacao<object>> AlterarStatusConsultaAsync(int id, Guid usuarioId, Action<Consulta> acaoDeAlteracao)
		{
			var (consulta, paciente, profissional) = await _consultaRepository.GetByIdComAgregadosAsync(id);
			if (consulta is null)
				return ResultadoOperacao<object>.Falha("Consulta não encontrada.", status: 404);

			if (consulta.UsuarioId != usuarioId)
				return ResultadoOperacao<object>.Falha("Você não tem permissão para alterar esta consulta.", status: 403);

			try
			{
				acaoDeAlteracao(consulta);
			}
			catch (InvalidOperationException ex)
			{
				return ResultadoOperacao<object>.Falha(ex.Message);
			}

			if (paciente == null || profissional == null)
				return ResultadoOperacao<object>.Falha("Não foi possível encontrar o paciente ou profissional associado para gerar o log.");

			await LogConsultaAsync(consulta.Id, consulta, paciente, profissional);

			var estadosFinais = new[]
			{
				(int)EnumStatusConsulta.Realizada,
				(int)EnumStatusConsulta.Cancelada,
				(int)EnumStatusConsulta.PacienteNaoCompareceu,
				(int)EnumStatusConsulta.ProfissionalNaoCompareceu
			};

			bool sucesso;
			if (estadosFinais.Contains((int)consulta.Status))
			{
				sucesso = await _consultaRepository.DeleteAsync(consulta.Id);
			}
			else
			{
				sucesso = await _consultaRepository.UpdateAsync(consulta);
			}

			if (!sucesso)
				return ResultadoOperacao<object>.Falha("Erro ao persistir a alteração da consulta no banco de dados.");

			return ResultadoOperacao<object>.Ok("Status da consulta alterado com sucesso.");
		}

		private async Task LogConsultaAsync(int consultaId, Consulta consulta, Paciente paciente, Profissional profissional)
		{
			var log = new ConsultaLog
			{
				ConsultaId = consultaId,
				UsuarioId = consulta.UsuarioId,
				PacienteId = paciente.Id,
				ProfissionalId = profissional.Id,
				NomePaciente = paciente.NomeCompleto,
				NomeProfissional = profissional.NomeCompleto,
				DataHoraInicio = consulta.DataHoraInicio,
				DataHoraFim = consulta.DataHoraFim,
				Motivo = consulta.Motivo,
				Observacao = consulta.Observacao,
				Status = (int)consulta.Status,
				DataLog = DateTime.UtcNow
			};

			await _consultaLogRepository.AddAsync(log);
		}

		#endregion
	}
}
