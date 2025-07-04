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
