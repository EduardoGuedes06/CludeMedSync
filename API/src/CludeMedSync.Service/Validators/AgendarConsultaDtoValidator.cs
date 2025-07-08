using CludeMedSync.Service.Models.Request;
using FluentValidation;

namespace CludeMedSync.Service.Validators
{
	public class AgendarConsultaRequestValidator : AbstractValidator<AgendarConsultaRequest>
	{
		public AgendarConsultaRequestValidator()
		{
			RuleFor(x => x.PacienteId)
				.NotEmpty().WithMessage("O ID do paciente é obrigatório.")
				.GreaterThan(0).WithMessage("O ID do paciente deve ser um número válido.");

			RuleFor(x => x.ProfissionalId)
				.NotEmpty().WithMessage("O ID do profissional é obrigatório.")
				.GreaterThan(0).WithMessage("O ID do profissional deve ser um número válido.");

			RuleFor(x => x.DataHoraInicio)
				.NotEmpty().WithMessage("A data e hora de início da consulta são obrigatórias.")
				.Must(BeAValidDate).WithMessage("A data da consulta não pode ser no passado.");

			RuleFor(x => x.Motivo)
				.MaximumLength(500).WithMessage("O motivo da consulta deve ter no máximo 500 caracteres.");
		}
		private bool BeAValidDate(DateTime date)
		{
			return date >= DateTime.Now;
		}
	}
}
