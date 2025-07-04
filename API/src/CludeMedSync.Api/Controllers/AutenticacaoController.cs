using CludeMedSync.Service.Common;
using CludeMedSync.Service.DTOs;
using CludeMedSync.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Controllers
{
	/// <summary>
	/// Controller responsável pela autenticação.
	/// </summary>
	/// 
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		/// <summary>
		/// Realiza o cadastro de usuario.
		/// </summary>
		/// <returns>ResultadoOperacao.</returns>
		/// <response code="200">Retorna o Resultado da Operacao</response>
		/// <response code="404">Retorna o Resultado da Operacao</response>
		[HttpPost("register")]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status200OK)]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
			var result = await _authService.RegisterAsync(registerDto);
			if (!result.Sucesso)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		/// <summary>
		/// Realiza o login do usuario e retorna o token.
		/// </summary>
		/// <returns>ResultadoOperacao.</returns>
		/// <response code="200">Retorna o Resultado da Operacao</response>
		/// <response code="404">Retorna o Resultado da Operacao</response>
		[HttpPost("login")]
		[ProducesResponseType(typeof(ResultadoOperacao<>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResultadoOperacao<TokenResponseDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var result = await _authService.LoginAsync(loginDto);
			if (!result.Sucesso)
			{
				return Unauthorized(result);
			}
			return Ok(result);
		}
	}
}
