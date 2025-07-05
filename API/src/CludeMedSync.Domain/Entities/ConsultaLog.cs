using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Entities
{
	public class ConsultaLog
	{
		public int Id { get; set; }
		public Guid UsuarioId { get; set; }
		public int ConsultaId { get; set; }
		public int PacienteId { get; set; }
		public int ProfissionalId { get; set; }
		public string NomePaciente { get; set; } = string.Empty;
		public string NomeProfissional { get; set; } = string.Empty;
		public string? EspecialidadeProfissional { get; set; }
		public DateTime DataHoraInicio { get; set; }
		public DateTime DataHoraFim { get; set; }
		public string? Motivo { get; set; }
		public string Status { get; set; } = string.Empty;
		public DateTime DataLog { get; set; }
	}
}
