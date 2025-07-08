using CludeMedSync.Models.Request;
using FluentValidation;

namespace CludeMedSync.Service.Validators
{
	public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
	{
		public RefreshTokenRequestValidator()
		{
			RuleFor(x => x.RefreshToken)
				.NotEmpty().WithMessage("O Refresh Token é obrigatório.");
		}
	}
}
