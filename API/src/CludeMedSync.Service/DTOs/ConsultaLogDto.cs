using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.DTOs
{
	public record ConsultaLogDto(
		int Id,
		Guid UsuarioId,
		int ConsultaId,
		string NomePaciente,
		string NomeProfissional,
		DateTime DataHoraInicio,
		string Status,
		string? motivo,
		string observacao,
		DateTime DataLog
	);
}
