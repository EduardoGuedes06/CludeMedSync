using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;

namespace CludeMedSync.Service.Services
{
	public class ConsultaService : IConsultaService
	{
		private readonly IConsultaRepository _consultaRepository;
		private readonly IPacienteRepository _pacienteRepository;
		private readonly IProfissionalRepository _profissionalRepository;

		public ConsultaService(
			IConsultaRepository consultaRepository,
			IPacienteRepository pacienteRepository,
			IProfissionalRepository profissionalRepository)
		{
			_consultaRepository = consultaRepository;
			_pacienteRepository = pacienteRepository;
			_profissionalRepository = profissionalRepository;
		}

		public async Task<ConsultaDto> AgendarAsync(AgendarConsultaDto dto)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(dto.PacienteId)
				?? throw new KeyNotFoundException("Paciente não encontrado.");

			var profissional = await _profissionalRepository.GetByIdAsync(dto.ProfissionalId)
				?? throw new KeyNotFoundException("Profissional não encontrado.");

			if (await _consultaRepository.ExisteConsultaNoMesmoDiaAsync(dto.PacienteId, dto.ProfissionalId, dto.DataHoraInicio))
				throw new InvalidOperationException("Este paciente já possui uma consulta com este profissional no mesmo dia.");

			if (await _consultaRepository.ExisteConsultaNoMesmoHorarioAsync(dto.ProfissionalId, dto.DataHoraInicio))
				throw new InvalidOperationException("O profissional não está disponível neste horário.");
			var novaConsulta = Consulta.Agendar(dto.PacienteId, dto.ProfissionalId, dto.DataHoraInicio, dto.Motivo);

			var novoId = await _consultaRepository.AddAsync(novaConsulta);

			return new ConsultaDto(
				novoId,
				paciente.Id,
				paciente.NomeCompleto,
				profissional.Id,
				profissional.NomeCompleto,
				novaConsulta.DataHoraInicio,
				novaConsulta.DataHoraFim,
				novaConsulta.Status
			);
		}

		public async Task<bool> CancelarAsync(int id)
		{
			var consulta = await _consultaRepository.GetByIdAsync(id);
			if (consulta is null) return false;
			consulta.Cancelar();

			return await _consultaRepository.UpdateAsync(consulta);
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
	}
}
