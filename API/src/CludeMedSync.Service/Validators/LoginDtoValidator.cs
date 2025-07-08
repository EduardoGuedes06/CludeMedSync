using CludeMedSync.Models.Request;
using FluentValidation;

namespace CludeMedSync.Service.Validators
{
	public class LoginDtoValidator : AbstractValidator<LoginRequest>
	{
		public LoginDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("O campo Email é obrigatório.")
				.EmailAddress().WithMessage("O formato do email é inválido.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("O campo Password é obrigatório.");
		}
	}
}
