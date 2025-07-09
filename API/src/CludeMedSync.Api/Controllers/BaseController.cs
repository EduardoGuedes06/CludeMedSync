using CludeMedSync.Service.Common;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
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
					throw new InvalidOperationException("Token inválido ou não contém um ID de utilizador válido.");

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
					throw new InvalidOperationException("Token inválido ou não contém a claim de nome do utilizador.");

				_usuarioNome = claimName;
				return _usuarioNome;
			}
		}

		protected static object? ParseFiltros(string? filtroString)
		{
			if (string.IsNullOrWhiteSpace(filtroString))
				return null;

			var keyValues = filtroString
				.Split(';', StringSplitOptions.RemoveEmptyEntries)
				.Select(pair => pair.Split(':', 2)) 
				.Where(parts => parts.Length == 2)
				.ToDictionary(
					parts => parts[0].Trim(),
					parts => (object)parts[1].Trim()
				);

			IDictionary<string, object> expando = new ExpandoObject();
			foreach (var kv in keyValues)
			{
				if (bool.TryParse(kv.Value.ToString(), out var boolVal))
					expando[kv.Key] = boolVal;
				else if (int.TryParse(kv.Value.ToString(), out var intVal))
					expando[kv.Key] = intVal;
				else if (DateTime.TryParse(kv.Value.ToString(), out var dateVal))
					expando[kv.Key] = dateVal;
				else
					expando[kv.Key] = kv.Value;
			}
			return expando;
		}
	}
}
