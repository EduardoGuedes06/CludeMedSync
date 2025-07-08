namespace CludeMedSync.Models.Response
{
    public record ProfissionalResponse(
        int Id,
        string NomeCompleto,
        string Especialidade,
        string CRM,
        string Email,
        string? Telefone
    );
}