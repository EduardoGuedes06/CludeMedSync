using CludeMedSync.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Validators
{
	public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
	{
		public RefreshTokenDtoValidator()
		{
			RuleFor(x => x.RefreshToken)
				.NotEmpty().WithMessage("O Refresh Token é obrigatório.");
		}
	}
}
