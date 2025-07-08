using CludeMedSync.Domain.Entities.Pagination;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.Interfaces;
using CludeMedSync.Service.Models.Request;
using CludeMedSync.Service.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Controllers
{
	/// <summary>
	/// Controller responsável pelas operações de consultas médicas.
	/// </summary>

	//[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class ConsultasController : BaseController
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
		[ProducesResponseType(typeof(PagedResult<ConsultaCompleta>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll(
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10,
		[FromQuery] string? orderBy = null,
		[FromQuery] bool orderByDesc = false)
		{
			var pacientesPaginados = await _consultaService.ObterConsultasPaginadoAsync(
				page,
				pageSize,
				filtros: null,
				orderBy: orderBy,
				orderByDesc: orderByDesc);
			return Ok(pacientesPaginados);
		}

		/// <summary>
		/// Retorna todo o historico de consultas.
		/// </summary>
		/// <returns>Lista de consultas.</returns>
		/// <response code="200">Retorna a lista de consultas</response>
		/// <response code="404"></response>
		/// <response code="500">Erro interno</response>
		[HttpGet("log")]
		[ProducesResponseType(typeof(PagedResult<ConsultaLogResponse>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllLog(
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10,
		[FromQuery] string? orderBy = null,
		[FromQuery] bool orderByDesc = false)
		{
			var logPacientesPaginados = await _consultaService.ObterConsultasLogPaginadoAsync(
				page,
				pageSize,
				filtros: null,
				orderBy: orderBy,
				orderByDesc: orderByDesc);
			return Ok(logPacientesPaginados);
		}

		/// <summary>
		/// Retorna o historio de uma Consulta.
		/// </summary>
		/// <returns>Lista de consultas.</returns>
		/// <response code="200">Retorna a lista de consultas</response>
		/// <response code="404"></response>
		/// <response code="500">Erro interno</response>
		[HttpGet("log/{idConsulta}")]
		[ProducesResponseType(typeof(PagedResult<ConsultaLogResponse>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllLog(
		[FromQuery] int idConsulta = 1,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10,
		[FromQuery] string? orderBy = null,
		[FromQuery] bool orderByDesc = false)
		{
			var logPacientesPaginados = await _consultaService.ObterConsultasLogPaginadoAsync(
				page,
				pageSize,
				filtros: new { ConsultaId = idConsulta },
				orderBy: orderBy,
				orderByDesc: orderByDesc);
			return Ok(logPacientesPaginados);
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
		public async Task<IActionResult> GetById(int id)
		{
			var consulta = await _consultaService.GetByIdAsync(id);
			return consulta == null ? NotFound() : Ok(consulta);
		}

		/// <summary>
		/// Agenda uma nova consulta.
		/// </summary>
		/// <param name="ConsultaResponse">Dados da consulta a ser agendada.</param>
		/// <returns>Consulta agendada.</returns>
		/// <response code="201">Consulta agendada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPost("agendar")]
		[ProducesResponseType(typeof(ResultadoOperacao<ConsultaResponse>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Agendar([FromBody] AgendarConsultaRequest ConsultaResponse)
		{
			var resultado = await _consultaService.AgendarAsync(ConsultaResponse, UsuarioId);
			if (!resultado.Sucesso)
				return BadRequest(resultado);

			return Ok(resultado);
		}


		/// <summary>
		/// Confirma uma consulta agendada.
		/// </summary>
		/// <param name="id">Dados da consulta a ser agendada.</param>
		/// <returns>Status da operação.</returns>
		/// <response code="200">Consulta atualizada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPatch("confirmar/{id}")]
		[ProducesResponseType(typeof(ResultadoOperacao<ConsultaResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Confirmar([FromRoute] int id)
		{
			var resultado = await _consultaService.ConfirmarAsync(id, UsuarioId);
			if (!resultado.Sucesso)
				return StatusCode(resultado.Status ?? 400, resultado);

			return Ok(resultado);
		}

		/// <summary>
		/// Inicia uma consulta confirmada.
		/// </summary>
		/// <param name="id">O ID da consulta.</param>		
		/// <response code="200">Consulta iniciada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPatch("iniciar/{id}")]
		[ProducesResponseType(typeof(ResultadoOperacao<ConsultaResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Iniciar([FromRoute] int id)
		{
			var resultado = await _consultaService.IniciarAsync(id, UsuarioId);
			if (!resultado.Sucesso)
				return StatusCode(resultado.Status ?? 400, resultado);

			return Ok(resultado);
		}

		/// <summary>
		/// Finaliza uma consulta que está em andamento.
		/// </summary>
		/// <param name="id">O ID da consulta.</param>
		/// <response code="200">Consulta Finalizada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPatch("finalizar/{id}")]
		[ProducesResponseType(typeof(ResultadoOperacao<ConsultaResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Finalizar([FromRoute] int id)
		{
			var resultado = await _consultaService.FinalizarAsync(id, UsuarioId);
			if (!resultado.Sucesso)
				return StatusCode(resultado.Status ?? 400, resultado);

			return Ok(resultado);
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
		[HttpPatch("cancelar/{id}")]
		[ProducesResponseType(typeof(ResultadoOperacao<ConsultaResponse>), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Cancelar([FromRoute] int id)
		{

			var resultado = await _consultaService.CancelarAsync(id, UsuarioId);

			if (!resultado.Sucesso)
				return BadRequest(resultado);

			return NoContent();
		}

		/// <summary>
		/// Marca uma consulta para a qual o paciente não compareceu.
		/// </summary>
		/// <param name="id">O ID da consulta.</param>
		/// <response code="200">Consulta atualizada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPatch("paciente-nao-compareceu/{id}")]
		[ProducesResponseType(typeof(ResultadoOperacao<ConsultaResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> MarcarComoPacienteNaoCompareceu([FromRoute] int id)
		{
			var resultado = await _consultaService.MarcarComoPacienteNaoCompareceuAsync(id, UsuarioId);
			if (!resultado.Sucesso)
				return StatusCode(resultado.Status ?? 400, resultado);

			return Ok(resultado);
		}

		/// <summary>
		/// Marca uma consulta para a qual o profissional não compareceu.
		/// </summary>
		/// <param name="id">O ID da consulta.</param>
		/// <response code="200">Consulta atualizada com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Consulta não encontrada</response>
		/// <response code="500">Erro interno</response>
		[HttpPatch("profissional-nao-compareceu/{id}")]
		[ProducesResponseType(typeof(ResultadoOperacao<ConsultaResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> MarcarComoProfissionalNaoCompareceu([FromRoute] int id)
		{
			var resultado = await _consultaService.MarcarComoProfissionalNaoCompareceuAsync(id, UsuarioId);
			if (!resultado.Sucesso)
				return StatusCode(resultado.Status ?? 400, resultado);

			return Ok(resultado);
		}
	}

}
