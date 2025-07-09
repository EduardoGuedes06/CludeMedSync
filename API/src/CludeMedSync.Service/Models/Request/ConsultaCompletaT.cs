using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Models.Request
{
	public class ConsultaCompletaT
	{
		public int ConsultaId { get; set; }

		// co.UsuarioId AS UsuarioId -> Corresponde ao Guid do banco
		public Guid UsuarioId { get; set; }

		// co.PacienteId AS PacienteId
		public int PacienteId { get; set; }

		// COALESCE(p.NomeCompleto, ...) AS PacienteNome
		public string PacienteNome { get; set; }

		// co.ProfissionalId AS ProfissionalId
		public int ProfissionalId { get; set; }

		// COALESCE(pr.NomeCompleto, ...) AS ProfissionalNome
		public string ProfissionalNome { get; set; }

		// co.DataHoraInicio AS DataHoraInicio
		public DateTime DataHoraInicio { get; set; }

		// co.DataHoraFim AS DataHoraFim -> Anulável (DateTime?) para aceitar valores NULL do banco
		public DateTime? DataHoraFim { get; set; }

		// CAST(co.Status AS CHAR) AS Status -> string para receber o resultado do CAST
		public string Status { get; set; }

		// co.Observacao AS Observacao -> Anulável (string?) para aceitar valores NULL
		public string? Observacao { get; set; }
	}
}
