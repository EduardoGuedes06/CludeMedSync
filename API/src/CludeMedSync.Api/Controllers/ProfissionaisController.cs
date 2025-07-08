using CludeMedSync.Models.Request;
using CludeMedSync.Models.Response;
using CludeMedSync.Service.Common;
using CludeMedSync.Service.Interfaces;
using CludeMedSync.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Controllers
{
	/// <summary>
	/// Controller responsável pelas operações com profissionais da saúde.
	/// </summary>

	//[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class ProfissionaisController : ControllerBase
	{
		private readonly IProfissionalService _profissionalService;

		public ProfissionaisController(IProfissionalService profissionalService)
		{
			_profissionalService = profissionalService;
		}

		/// <summary>
		/// Retorna todos os profissionais cadastrados.
		/// </summary>
		/// <returns>Lista de profissionais.</returns>
		/// <response code="200">Retorna a lista de profissionais</response>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<ProfissionalResponse>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll(
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10,
		[FromQuery] string? orderBy = null,
		[FromQuery] bool orderByDesc = false,
		[FromQuery] bool ativo = true)
		{
			var proffisionaisPaginados = await _profissionalService.ObterPaginadoGenericoAsync(
				page,
				pageSize,
				filtros: null,
				orderBy: orderBy,
				orderByDesc: orderByDesc,
				tipoDto: typeof(ProfissionalResponse));
			return Ok(proffisionaisPaginados);
		}

		/// <summary>
		/// Retorna os dados de um profissional específico.
		/// </summary>
		/// <param name="id">ID do profissional.</param>
		/// <returns>Profissional correspondente.</returns>
		/// <response code="200">Profissional encontrado</response>
		/// <response code="404">Profissional não encontrado</response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ProfissionalResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var profissional = await _profissionalService.GetByIdAsync(id);
			return profissional == null ? NotFound() : Ok(profissional);
		}

		/// <summary>
		/// Cadastra um novo profissional.
		/// </summary>
		/// <param name="ProfissionalResponse">Dados do profissional a ser criado.</param>
		/// <returns>Profissional criado.</returns>
		/// <response code="201">Profissional criado com sucesso</response>
		/// <response code="400">Dados inválidos ou conflito de cadastro</response>
		[HttpPost]
		[ProducesResponseType(typeof(ResultadoOperacao<ProfissionalResponse>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] ProfissionalRequest PacienteResponse)
		{
			var resultado = await _profissionalService.CreateAsync(PacienteResponse);

			if (!resultado.Sucesso)
				return BadRequest(resultado);

			return Ok(resultado);
		}


		/// <summary>
		/// Atualiza os dados de um profissional existente.
		/// </summary>
		/// <param name="id">ID do profissional.</param>
		/// <param name="ProfissionalResponse">Novos dados do profissional.</param>
		/// <returns>Status da operação.</returns>
		/// <response code="204">Profissional atualizado com sucesso</response>
		/// <response code="404">Profissional não encontrado</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Update(int id, [FromBody] ProfissionalRequest ProfissionalResponse)
		{
			var sucesso = await _profissionalService.UpdateAsync(id, ProfissionalResponse);
			return sucesso ? NoContent() : NotFound();
		}

		/// <summary>
		/// Desativa ou Remove um profissional do sistema.
		/// </summary>
		/// <param name="id">ID do profissional a ser desativado/removido.</param>
		/// <returns>Status da exclusão.</returns>
		/// <response code="204">Profissional desativado/removido com sucesso</response>
		/// <response code="404">Profissional não encontrado</response>
		/// <response code="400">Dados Invalidos ou conflito na deleção</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResultadoOperacao<ProfissionalResponse>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Delete(int id)
		{
			var resultado = await _profissionalService.DeleteAsync(id);

			if (!resultado.Sucesso)
				return BadRequest(new { Sucesso = false, mensagem = resultado.Mensagem, dados = resultado.Dados });

			return NoContent();
		}

	}


}
