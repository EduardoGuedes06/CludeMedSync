using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Controllers
{
	/// <summary>
	/// Controller responsável pelas operações de consultas médicas.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class ConsultasController : ControllerBase
	{
		private readonly IConsultaService _consultaService;

		public ConsultasController(IConsultaService consultaService)
		{
			_consultaService = consultaService;
		}

		/// <summary>
		/// Retorna todas as consultas cadastradas.
		/// </summary>
		/// <returns>Lista de consultas.</returns>
		/// <response code="200">Retorna a lista de consultas</response>
		/// <response code="404"></response>
		/// <response code="500">Erro interno</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var consultas = await _consultaService.GetAllAsync();
			return Ok(consultas);
		}

		/// <summary>
		/// Retorna uma consulta específica pelo ID.
		/// </summary>
		/// <param name="id">ID da consulta.</param>
		/// <returns>Dados da consulta.</returns>
		/// <response code="200">Consulta encontrada</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var consulta = await _consultaService.GetByIdAsync(id);
			return consulta == null ? NotFound() : Ok(consulta);
		}

		/// <summary>
		/// Agenda uma nova consulta.
		/// </summary>
		/// <param name="consultaDto">Dados da consulta a ser agendada.</param>
		/// <returns>Consulta agendada.</returns>
		/// <response code="201">Consulta agendada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPost("agendar")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Agendar([FromBody] AgendarConsultaDto consultaDto)
		{
			try
			{
				var novaConsulta = await _consultaService.AgendarAsync(consultaDto);
				return CreatedAtAction(nameof(GetById), new { id = novaConsulta.Id }, novaConsulta);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Cancela uma consulta existente.
		/// </summary>
		/// <param name="id">ID da consulta a ser cancelada.</param>
		/// <returns>Status da operação.</returns>
		/// <response code="204">Consulta cancelada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPatch("{id}/cancelar")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Cancelar(int id)
		{
			var sucesso = await _consultaService.CancelarAsync(id);
			return sucesso ? NoContent() : NotFound();
		}
	}

}
