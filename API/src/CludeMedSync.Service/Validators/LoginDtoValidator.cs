using CludeMedSync.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Validators
{
	public class LoginDtoValidator : AbstractValidator<LoginDto>
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
