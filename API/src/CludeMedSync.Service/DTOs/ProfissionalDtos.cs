using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.DTOs
{
	public record CreateProfissionalDto(
		string NomeCompleto,
		string Especialidade,
		string CRM,
		string Email,
		string? Telefone
	);

	public record ProfissionalDto(
		int Id,
		string NomeCompleto,
		string Especialidade,
		string CRM,
		string Email,
		string? Telefone
	);
}
