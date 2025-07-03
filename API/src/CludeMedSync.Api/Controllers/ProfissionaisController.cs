using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Controllers
{
	/// <summary>
	/// Controller responsável pelas operações com profissionais da saúde.
	/// </summary>
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var profissional = await _profissionalService.GetAllAsync();
			return Ok(profissional);
		}

		/// <summary>
		/// Retorna os dados de um profissional específico.
		/// </summary>
		/// <param name="id">ID do profissional.</param>
		/// <returns>Profissional correspondente.</returns>
		/// <response code="200">Profissional encontrado</response>
		/// <response code="404">Profissional não encontrado</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var profissional = await _profissionalService.GetByIdAsync(id);
			return profissional == null ? NotFound() : Ok(profissional);
		}

		/// <summary>
		/// Cadastra um novo profissional.
		/// </summary>
		/// <param name="profissionalDto">Dados do profissional a ser criado.</param>
		/// <returns>Profissional criado.</returns>
		/// <response code="201">Profissional criado com sucesso</response>
		/// <response code="400">Dados inválidos ou conflito de cadastro</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] CreateProfissionalDto profissionalDto)
		{
			try
			{
				var novoProfissional = await _profissionalService.CreateAsync(profissionalDto);
				return CreatedAtAction(nameof(GetById), new { id = novoProfissional.Id }, novoProfissional);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Atualiza os dados de um profissional existente.
		/// </summary>
		/// <param name="id">ID do profissional.</param>
		/// <param name="profissionalDto">Novos dados do profissional.</param>
		/// <returns>Status da operação.</returns>
		/// <response code="204">Profissional atualizado com sucesso</response>
		/// <response code="404">Profissional não encontrado</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, [FromBody] CreateProfissionalDto profissionalDto)
		{
			var sucesso = await _profissionalService.UpdateAsync(id, profissionalDto);
			return sucesso ? NoContent() : NotFound();
		}

		/// <summary>
		/// Remove um profissional do sistema.
		/// </summary>
		/// <param name="id">ID do profissional a ser removido.</param>
		/// <returns>Status da exclusão.</returns>
		/// <response code="204">Profissional removido com sucesso</response>
		/// <response code="404">Profissional não encontrado</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			var sucesso = await _profissionalService.DeleteAsync(id);
			return sucesso ? NoContent() : NotFound();
		}
	}


}
