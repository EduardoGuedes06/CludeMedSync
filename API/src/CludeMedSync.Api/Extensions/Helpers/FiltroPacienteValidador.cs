using CludeMedSync.Domain.Models;

namespace CludeMedSync.Api.Extensions.Helpers
{
	public static class FiltroPacienteValidator
	{
		public static readonly Dictionary<string, Type> CamposPermitidos = new()
		{
			{ $"{nameof(Paciente.Id)}", typeof(int) },
			{ $"{nameof(Paciente.NomeCompleto)}", typeof(string) },
			{ $"{nameof(Paciente.DataNascimento)}", typeof(DateTime) },
			{ $"{nameof(Paciente.CPF)}", typeof(string) },
			{ $"{nameof(Paciente.Email)}", typeof(string) },
			{ $"{nameof(Paciente.Telefone)}", typeof(string) },
			{ $"Ativo", typeof(bool) }
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
					var convertido = Convert.ChangeType(filtro.Value, tipoEsperado);
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
