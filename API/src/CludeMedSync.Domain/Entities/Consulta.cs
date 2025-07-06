using CludeMedSync.Domain.Entities.Utils.Enums;
namespace CludeMedSync.Domain.Models;

public class Consulta
{
	public int Id { get; set; }
	public Guid UsuarioId { get; set; }
	public int PacienteId { get; set; }
	public int ProfissionalId { get; set; }
	public DateTime DataHoraInicio { get; set; }
	public DateTime DataHoraFim { get; set; }
	public string? Motivo { get; set; }
	public string? Observacao { get; set; }
	public int Status { get; set; }

	public Consulta() { }

	public static Consulta Agendar(Guid usuarioId, int pacienteId, int profissionalId, DateTime dataHoraInicio, string? motivo, string? observacao)
	{
		ValidarHorario(dataHoraInicio);
		return new Consulta
		{
			UsuarioId = usuarioId,
			PacienteId = pacienteId,
			ProfissionalId = profissionalId,
			DataHoraInicio = dataHoraInicio,
			DataHoraFim = dataHoraInicio.AddMinutes(30),
			Motivo = motivo,
			Observacao = observacao,
			Status = (int)EnumStatusConsulta.Agendada
		};
	}

	public void AtualizarDados(DateTime novaDataHora, string? novoMotivo, string? novaObservacao)
	{
		if (Status != (int)EnumStatusConsulta.Agendada && Status != (int)EnumStatusConsulta.Confirmada)
			throw new InvalidOperationException("Só é possível alterar os dados de uma consulta agendada ou confirmada.");

		ValidarHorario(novaDataHora);
		DataHoraInicio = novaDataHora;
		DataHoraFim = novaDataHora.AddMinutes(30);
		Motivo = novoMotivo;
		Observacao = novaObservacao;
	}
	public void Confirmar()
	{
		if (Status != (int)EnumStatusConsulta.Agendada)
			throw new InvalidOperationException("Só é possível confirmar uma consulta que está agendada.");
		Status = (int)EnumStatusConsulta.Confirmada;
	}

	public void Iniciar()
	{
		if (Status != (int)EnumStatusConsulta.Agendada && Status != (int)EnumStatusConsulta.Confirmada)
			throw new InvalidOperationException("Só é possível iniciar uma consulta agendada ou confirmada.");
		Status = (int)EnumStatusConsulta.EmAndamento;
	}

	public void Finalizar()
	{
		if (Status != (int)EnumStatusConsulta.EmAndamento)
			throw new InvalidOperationException("Só é possível finalizar uma consulta que está em andamento.");
		Status = (int)EnumStatusConsulta.Realizada;
	}

	public void Cancelar()
	{
		if (Status == (int)EnumStatusConsulta.Realizada || Status == (int)EnumStatusConsulta.Cancelada)
			throw new InvalidOperationException("Não é possível cancelar uma consulta que já foi realizada ou cancelada.");
		Status = (int)EnumStatusConsulta.Cancelada;
	}

	public void MarcarComoPacienteNaoCompareceu()
	{
		if (Status != (int)EnumStatusConsulta.Agendada && Status != (int)EnumStatusConsulta.Confirmada)
			throw new InvalidOperationException("Só é possível marcar como 'não compareceu' uma consulta agendada ou confirmada.");
		Status = (int)EnumStatusConsulta.PacienteNaoCompareceu;
	}

	public void MarcarComoProfissionalNaoCompareceu()
	{
		if (Status != (int)EnumStatusConsulta.Agendada && Status != (int)EnumStatusConsulta.Confirmada)
			throw new InvalidOperationException("Só é possível marcar como 'não compareceu' uma consulta agendada ou confirmada.");
		Status = (int)EnumStatusConsulta.ProfissionalNaoCompareceu;
	}

	private static void ValidarHorario(DateTime dataHora)
	{
		if (dataHora.DayOfWeek < DayOfWeek.Monday || dataHora.DayOfWeek > DayOfWeek.Friday)
			throw new ArgumentException("Consultas só podem ser agendadas de segunda a sexta-feira.");

		if (dataHora.TimeOfDay < TimeSpan.FromHours(8) || dataHora.TimeOfDay >= TimeSpan.FromHours(18))
			throw new ArgumentException("Consultas só podem ser agendadas entre 08:00 e 18:00.");
	}
}
