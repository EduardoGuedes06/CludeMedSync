using CludeMedSync.Models.Request;
using FluentValidation;

namespace CludeMedSync.Service.Validators
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		public RegisterRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("O campo Email é obrigatório.")
				.EmailAddress().WithMessage("O formato do email é inválido.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("O campo Password é obrigatório.");

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage("O campo de confirmação da Password é obrigatório.")
				.Equal(x => x.Password).WithMessage("As palavras-passe não coincidem.");
		}
	}

}
