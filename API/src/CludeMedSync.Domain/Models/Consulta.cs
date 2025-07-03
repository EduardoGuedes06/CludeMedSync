using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Models
{
	public class Consulta
	{
		public int Id { get; private set; }
		public int PacienteId { get; private set; }
		public int ProfissionalId { get; private set; }
		public DateTime DataHoraInicio { get; private set; }
		public DateTime DataHoraFim { get; private set; }
		public string? Motivo { get; private set; }
		public string Status { get; private set; }

		protected Consulta() { }

		public static Consulta Agendar(int pacienteId, int profissionalId, DateTime dataHoraInicio, string? motivo)
		{
			ValidarHorario(dataHoraInicio);

			return new Consulta
			{
				PacienteId = pacienteId,
				ProfissionalId = profissionalId,
				DataHoraInicio = dataHoraInicio,
				DataHoraFim = dataHoraInicio.AddMinutes(30),
				Motivo = motivo,
				Status = "Agendada"
			};
		}

		public void Cancelar()
		{
			if (Status == "Realizada")
				throw new InvalidOperationException("Não é possível cancelar uma consulta já realizada.");
			Status = "Cancelada";
		}

		private static void ValidarHorario(DateTime dataHora)
		{
			if (dataHora.DayOfWeek < DayOfWeek.Monday || dataHora.DayOfWeek > DayOfWeek.Friday)
				throw new ArgumentException("Consultas só podem ser agendadas de segunda a sexta-feira.");

			if (dataHora.TimeOfDay < TimeSpan.FromHours(8) || dataHora.TimeOfDay >= TimeSpan.FromHours(18))
				throw new ArgumentException("Consultas só podem ser agendadas entre 08:00 e 18:00.");
		}
	}
}