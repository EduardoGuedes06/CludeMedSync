using CludeMedSync.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CludeMedSync.Api.Controllers
{
	[ApiController]
	public abstract class BaseController : ControllerBase
	{
		public Guid UsuarioId { get; private set; } = Guid.Empty;
		public string UsuarioNome { get; private set; } = string.Empty;

		[NonAction]
		public void PreencherDadosUsuario(Guid usuarioId, string usuarioNome)
		{
			UsuarioId = usuarioId;
			UsuarioNome = usuarioNome;
		}
		public ResultadoOperacao<Object> ObterDadosUsuarioAutenticado()
		{
			var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var usuarioNome = User.FindFirst(ClaimTypes.Name)?.Value;

			if (usuarioId == null || string.IsNullOrEmpty(usuarioNome))
			{
				return ResultadoOperacao<Object>.Falha(
					"Utilizador não autenticado ou Usuario Inexistente. As claims de ID e Nome são obrigatórias.",
					status: 401
				);
			}

			PreencherDadosUsuario(Guid.Parse(usuarioId), usuarioNome);

			return ResultadoOperacao<Object>.Ok(
				"Utilizador validado com sucesso."
			);
		}
	}
}
