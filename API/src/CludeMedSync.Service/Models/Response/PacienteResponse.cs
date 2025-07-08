namespace CludeMedSync.Models.Response
{
    public record PacienteResponse(
        int Id,
        string NomeCompleto,
        DateTime DataNascimento,
        string CPF,
        string Email,
        string? Telefone
    );
}