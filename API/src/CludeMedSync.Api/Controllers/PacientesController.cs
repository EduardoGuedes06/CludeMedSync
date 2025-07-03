using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Controllers
{
	/// <summary>
	/// Controller responsável pelas operações relacionadas a pacientes.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class PacientesController : ControllerBase
	{
		private readonly IPacienteService _pacienteService;

		public PacientesController(IPacienteService pacienteService)
		{
			_pacienteService = pacienteService;
		}

		/// <summary>
		/// Retorna todos os pacientes cadastrados.
		/// </summary>
		/// <returns>Lista de pacientes.</returns>
		/// <response code="200">Retorna a lista de pacientes</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<PacienteDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var pacientes = await _pacienteService.GetAllAsync();
			return Ok(pacientes);
		}

		/// <summary>
		/// Retorna os dados de um paciente específico.
		/// </summary>
		/// <param name="id">ID do paciente.</param>
		/// <returns>Paciente correspondente.</returns>
		/// <response code="200">Paciente encontrado</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(PacienteDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var paciente = await _pacienteService.GetByIdAsync(id);
			return paciente == null ? NotFound() : Ok(paciente);
		}

		/// <summary>
		/// Cria um novo paciente.
		/// </summary>
		/// <param name="pacienteDto">Dados do paciente a ser criado.</param>
		/// <returns>Paciente criado.</returns>
		/// <response code="201">Paciente criado com sucesso</response>
		/// <response code="400">Dados inválidos ou paciente já existente</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] CreatePacienteDto pacienteDto)
		{
			try
			{
				var novoPaciente = await _pacienteService.CreateAsync(pacienteDto);
				return CreatedAtAction(nameof(GetById), new { id = novoPaciente.Id }, novoPaciente);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Atualiza os dados de um paciente existente.
		/// </summary>
		/// <param name="id">ID do paciente.</param>
		/// <param name="pacienteDto">Novos dados do paciente.</param>
		/// <returns>Status da operação.</returns>
		/// <response code="204">Paciente atualizado com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Paciente não Encontrado</response>
		/// <response code="500">Erro interno</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, [FromBody] CreatePacienteDto pacienteDto)
		{
			var sucesso = await _pacienteService.UpdateAsync(id, pacienteDto);
			return sucesso ? NoContent() : NotFound();
		}

		/// <summary>
		/// Remove um paciente do sistema.
		/// </summary>
		/// <param name="id">ID do paciente a ser excluído.</param>
		/// <returns>Status da exclusão.</returns>
		/// <response code="204">Paciente excluído com sucesso</response>
		/// <response code="404">Paciente não encontrado</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="500">Erro interno</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			var sucesso = await _pacienteService.DeleteAsync(id);
			return sucesso ? NoContent() : NotFound();
		}
	}

}
