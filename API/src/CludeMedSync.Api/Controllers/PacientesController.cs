using CludeMedSync.Api.Extensions.Helpers;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Models.Request;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Controllers
{
	/// <summary>
	/// Controller responsável pelas operações relacionadas a pacientes.
	/// </summary>
	
	//[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class PacientesController : BaseController
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
		[ProducesResponseType(typeof(ResultadoOperacao<PagedResult<PacienteResponse>>), StatusCodes.Status200OK)]
		[HttpGet]
		public async Task<IActionResult> GetAll(
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10,
		[FromQuery] string? filtros = null,
		[FromQuery] string? orderBy = null,
		[FromQuery] bool orderByDesc = false)
		{
			object? filtrosObj = null;
			if (!string.IsNullOrWhiteSpace(filtros))
			{
				filtrosObj = ParseFiltros(filtros) as IDictionary<string, object>;

				var (valido, erro) = FiltroPacienteValidator.ValidarFiltros((IDictionary<string, object>)filtrosObj!);
				if (!valido)
					return BadRequest(ResultadoOperacao<object>.Falha(erro!));
			}

			var pacientesPaginados = await _pacienteService.ObterPaginadoGenericoAsync(
				page,
				pageSize,
				filtros: filtrosObj,
				orderBy: orderBy,
				orderByDesc: orderByDesc,
				tipoDto: typeof(PacienteResponse));

			var resultado = ResultadoOperacao<object>.Ok("Pacientes obtidos com sucesso", pacientesPaginados);
			return Ok(resultado);
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
		[ProducesResponseType(typeof(PacienteResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetById(int id)
		{
			var paciente = await _pacienteService.GetByIdAsync(id);
			return paciente == null ? NotFound() : Ok(paciente);
		}

		/// <summary>
		/// Cria um novo paciente.
		/// </summary>
		/// <param name="PacienteResponse">Dados do paciente a ser criado.</param>
		/// <returns>Paciente criado.</returns>
		/// <response code="201">Paciente criado com sucesso</response>
		/// <response code="400">Dados inválidos ou paciente já existente</response>
		[HttpPost]
		[ProducesResponseType(typeof(ResultadoOperacao<PacienteResponse>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] PacienteRequest PacienteResponse)
		{
			var resultado = await _pacienteService.CreateAsync(PacienteResponse);

			if (!resultado.Sucesso)
				return BadRequest(resultado);

			return Ok(resultado);
		}

		/// <summary>
		/// Atualiza os dados de um paciente existente.
		/// </summary>
		/// <param name="id">ID do paciente.</param>
		/// <param name="PacienteResponse">Novos dados do paciente.</param>
		/// <returns>Status da operação.</returns>
		/// <response code="204">Paciente atualizado com sucesso</response>
		/// <response code="400">Dados inválidos</response>
		/// <response code="404">Paciente não Encontrado</response>
		/// <response code="500">Erro interno</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Update(int id, [FromBody] PacienteRequest PacienteResponse)
		{
			var sucesso = await _pacienteService.UpdateAsync(id, PacienteResponse);
			return sucesso ? NoContent() : NotFound();
		}

		/// <summary>
		/// Desativa/Remove um paciente do sistema.
		/// </summary>
		/// <param name="id">ID do paciente a ser desativado/removido.</param>
		/// <returns>Status da exclusão.</returns>
		/// <response code="204">Paciente desativado/removido com sucesso</response>
		/// <response code="404">Paciente não encontrado</response>
		/// <response code="400">Dados Invalidos ou conflito na deleção</response>
		/// <response code="500">Erro interno</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResultadoOperacao<PacienteResponse>), StatusCodes.Status400BadRequest)]

		public async Task<IActionResult> Delete(int id)
		{
			var resultado = await _pacienteService.DeleteAsync(id);

			if (!resultado.Sucesso)
				return BadRequest(new { Sucesso = false, mensagem = resultado.Mensagem, dados = resultado.Dados });

			return NoContent();
		}
	}

}
