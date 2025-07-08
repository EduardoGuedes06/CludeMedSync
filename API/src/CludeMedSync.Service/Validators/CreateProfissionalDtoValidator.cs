using CludeMedSync.Models.Request;
using FluentValidation;

namespace CludeMedSync.Service.Validators
{
	public class CreateProfissionalResponseValidator : AbstractValidator<ProfissionalRequest>
	{
		public CreateProfissionalResponseValidator()
		{
			RuleFor(x => x.NomeCompleto)
				.NotEmpty().WithMessage("O nome completo é obrigatório.")
				.MinimumLength(3).WithMessage("O nome completo deve ter no mínimo 3 caracteres.")
				.MaximumLength(200).WithMessage("O nome completo deve ter no máximo 200 caracteres.");

			RuleFor(x => x.Especialidade)
				.NotEmpty().WithMessage("A especialidade é obrigatória.")
				.MaximumLength(100).WithMessage("A especialidade deve ter no máximo 100 caracteres.");

			RuleFor(x => x.CRM)
				.NotEmpty().WithMessage("O CRM é obrigatório.")
				.MaximumLength(20).WithMessage("O CRM deve ter no máximo 20 caracteres.");

			RuleFor(x => x.Telefone)
				.NotEmpty().WithMessage("O Telefone é obrigatório.")
				.Matches(@"^\(\d{2}\)\s?\d{4,5}-\d{4}$")
				.WithMessage("O formato do telefone deve ser (XX) XXXXX-XXXX ou (XX) XXXX-XXXX.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("O e-mail é obrigatório.")
				.EmailAddress().WithMessage("O e-mail fornecido não é válido.");
		}
	}
}
