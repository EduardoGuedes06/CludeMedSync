using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Models.Request
{
	public record AgendarConsultaRequest(
		int PacienteId,
		int ProfissionalId,
		DateTime DataHoraInicio,
		string? Motivo,
		string? Observacao
	);

	public record AtualizarConsultaRequest(
		DateTime DataHoraInicio,
		string? Motivo,
		string? Observacao
	);
}
