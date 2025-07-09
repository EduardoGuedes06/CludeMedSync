namespace CludeMedSync.Api.Extensions.Helpers
{
	public static class FiltroConsultaValidator
	{
		public static readonly Dictionary<string, Type> CamposPermitidos = new()
	{
		{ "Id", typeof(int) },
		{ "ConsultaId", typeof(int) },
		{ "UsuarioId", typeof(Guid) },
		{ "PacienteId", typeof(int) },
		{ "ProfissionalId", typeof(int) },
		{ "DataHoraInicio", typeof(DateTime) },
		{ "DataHoraFim", typeof(DateTime) },
		{ "Motivo", typeof(string) },
		{ "Observacao", typeof(string) },
		{ "Status", typeof(int) }
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
