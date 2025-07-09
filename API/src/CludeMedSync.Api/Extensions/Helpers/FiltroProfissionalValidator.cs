using CludeMedSync.Domain.Models;

namespace CludeMedSync.Api.Extensions.Helpers
{
	public static class FiltroProfissionalValidator
	{
		public static readonly Dictionary<string, Type> CamposPermitidos = new()
	{
		{ $"{nameof(Profissional.Id)}", typeof(int) },
		{ $"{nameof(Profissional.NomeCompleto)}", typeof(string) },
		{ $"{nameof(Profissional.Especialidade)}", typeof(string) },
		{ $"{nameof(Profissional.CRM)}", typeof(string) },
		{ $"{nameof(Profissional.Email)}", typeof(string) },
		{ $"{nameof(Profissional.Telefone)}", typeof(string) },
		{ "Ativo", typeof(bool) }
	};

		public static (bool valido, string? erro) ValidarFiltros(IDictionary<string, object> filtros)
		{
			foreach (var filtro in filtros)
			{
				if (!CamposPermitidos.TryGetValue(filtro.Key, out var tipoEsperado))
				{
					return (false, $"Filtro inválido: {filtro.Key}");
				}

				try
				{
					_ = Convert.ChangeType(filtro.Value, tipoEsperado);
				}
				catch
				{
					return (false, $"Tipo inválido para o campo '{filtro.Key}'. Esperado: {tipoEsperado.Name}");
				}
			}

			return (true, null);
		}
	}

}
