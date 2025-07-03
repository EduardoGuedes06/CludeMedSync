using CludeMedSync.Service.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Validators
{
	public class CreateProfissionalDtoValidator : AbstractValidator<CreateProfissionalDto>
	{
		public CreateProfissionalDtoValidator()
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

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("O e-mail é obrigatório.")
				.EmailAddress().WithMessage("O e-mail fornecido não é válido.");
		}
	}
}
