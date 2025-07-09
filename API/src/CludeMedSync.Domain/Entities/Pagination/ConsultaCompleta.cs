namespace CludeMedSync.Domain.Entities.Pagination
{

	public class ConsultaCompleta
	{
		public int ConsultaId { get; set; }
		public string UsuarioId { get; set; }
		public int PacienteId { get; set; }
		public string PacienteNome { get; set; }
		public int ProfissionalId { get; set; }
		public string ProfissionalNome { get; set; }
		public DateTime DataHoraInicio { get; set; }
		public DateTime? DataHoraFim { get; set; }
		public int Status { get; set; }
		public string? Observacao { get; set; }
	}
}
