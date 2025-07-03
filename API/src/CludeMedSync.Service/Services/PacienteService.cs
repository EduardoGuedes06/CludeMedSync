using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Services
{
	public class PacienteService : IPacienteService
	{
		private readonly IPacienteRepository _pacienteRepository;

		public PacienteService(IPacienteRepository pacienteRepository)
		{
			_pacienteRepository = pacienteRepository;
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

		public async Task<PacienteDto> CreateAsync(CreatePacienteDto pacienteDto)
		{
			var pacienteExistente = await _pacienteRepository.GetByCpfAsync(pacienteDto.CPF);
			if (pacienteExistente != null)
			{
				throw new InvalidOperationException("Já existe um paciente cadastrado com este CPF.");
			}

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

			return new PacienteDto(paciente.Id, paciente.NomeCompleto, paciente.DataNascimento, paciente.CPF, paciente.Email, paciente.Telefone);
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

		public async Task<bool> DeleteAsync(int id)
		{
			var paciente = await _pacienteRepository.GetByIdAsync(id);
			if (paciente == null) return false;

			return await _pacienteRepository.DeleteAsync(id);
		}
	}
}
