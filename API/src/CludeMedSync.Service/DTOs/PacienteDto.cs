namespace CludeMedSync.Services.DTOs
{
	public record CreatePacienteDto(
		string NomeCompleto,
		DateTime DataNascimento,
		string CPF,
		string Email,
		string? Telefone
	);

	public record PacienteDto(
		int Id,
		string NomeCompleto,
		DateTime DataNascimento,
		string CPF,
		string Email,
		string? Telefone
	);
}
