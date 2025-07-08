namespace CludeMedSync.Service.Models.Response
{
	public record ConsultaResponse(
		int Id,
		Guid UsuarioId,
		int PacienteId,
		string NomePaciente,
		int ProfissionalId,
		string NomeProfissional,
		DateTime DataHoraInicio,
		DateTime DataHoraFim,
		string Status,
		string? Observacao
	);
}
