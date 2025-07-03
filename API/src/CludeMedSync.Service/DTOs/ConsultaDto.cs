using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.DTOs
{
	public record AgendarConsultaDto(
		int PacienteId,
		int ProfissionalId,
		DateTime DataHoraInicio,
		string? Motivo
	);

	public record ConsultaDto(
		int Id,
		int PacienteId,
		string NomePaciente,
		int ProfissionalId,
		string NomeProfissional,
		DateTime DataHoraInicio,
		DateTime DataHoraFim,
		string Status
	);
}
