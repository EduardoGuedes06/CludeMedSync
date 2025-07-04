using CludeMedSync.Service.DTOs;
using FluentValidation;

namespace CludeMedSync.Service.Validators
{
	public class CreatePacienteDtoValidator : AbstractValidator<CreatePacienteDto>
	{
		public CreatePacienteDtoValidator()
		{
			RuleFor(x => x.NomeCompleto)
				.NotEmpty().WithMessage("O nome completo é obrigatório.")
				.MinimumLength(3).WithMessage("O nome completo deve ter no mínimo 3 caracteres.")
				.MaximumLength(200).WithMessage("O nome completo deve ter no máximo 200 caracteres.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("O e-mail é obrigatório.")
				.EmailAddress().WithMessage("O e-mail fornecido não é válido.");

			RuleFor(x => x.CPF)
				.NotEmpty().WithMessage("O CPF é obrigatório.")
				.Length(11).WithMessage("O CPF deve ter 11 caracteres.")
				.Must(cpf => cpf.All(char.IsDigit)).WithMessage("O CPF deve conter apenas números.");

			RuleFor(x => x.Telefone)
				.NotEmpty().WithMessage("O Telefone é obrigatório.")
				.Matches(@"^\(\d{2}\)\s?\d{4,5}-\d{4}$")
				.WithMessage("O formato do telefone deve ser (XX) XXXXX-XXXX ou (XX) XXXX-XXXX.");

			RuleFor(x => x.DataNascimento)
				.NotEmpty().WithMessage("A data de nascimento é obrigatória.")
				.LessThan(DateTime.Now).WithMessage("A data de nascimento não pode ser no futuro.");
		}
	}
}
