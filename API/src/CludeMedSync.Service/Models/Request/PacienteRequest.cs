namespace CludeMedSync.Models.Request
{
    public record PacienteRequest(
        string NomeCompleto,
        DateTime DataNascimento,
        string CPF,
        string Email,
        string? Telefone
    );
}