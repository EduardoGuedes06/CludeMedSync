namespace CludeMedSync.Models.Request
{
    public record ProfissionalRequest(
        string NomeCompleto,
        string Especialidade,
        string CRM,
        string Email,
        string? Telefone
    );
}