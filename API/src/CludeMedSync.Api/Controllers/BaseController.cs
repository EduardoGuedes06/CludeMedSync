using CludeMedSync.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CludeMedSync.Api.Controllers
{
	[ApiController]
	public abstract class BaseController : ControllerBase
	{
		private Guid? _usuarioId;
		private string? _usuarioNome;

		protected Guid UsuarioId
		{
			get
			{
				if (_usuarioId.HasValue)
					return _usuarioId.Value;
				var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (!Guid.TryParse(claimId, out var parsedId))
				{
					throw new InvalidOperationException("Token inválido ou não contém um ID de utilizador válido.");
				}

				_usuarioId = parsedId;
				return _usuarioId.Value;
			}
		}

		protected string UsuarioNome
		{
			get
			{
				if (!string.IsNullOrEmpty(_usuarioNome))
					return _usuarioNome;

				var claimName = User.FindFirst(ClaimTypes.Name)?.Value;
				if (string.IsNullOrEmpty(claimName))
				{
					throw new InvalidOperationException("Token inválido ou não contém a claim de nome do utilizador.");
				}

				_usuarioNome = claimName;
				return _usuarioNome;
			}
		}
	}
}
