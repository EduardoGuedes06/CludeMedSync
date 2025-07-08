namespace CludeMedSync.Models.Response
{
    public record ConsultaLogResponse(
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