using CludeMedSync.Service.Common;
using CludeMedSync.Service.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace CludeMedSync.Api.Configuration
{
	public static class MvcConfig
	{
		public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
		{
			services.AddControllers()
				.AddDataAnnotationsLocalization();

			services.AddFluentValidationAutoValidation();
			services.AddValidatorsFromAssemblyContaining<CreatePacienteResponseValidator>();

			services.AddLocalization(options => options.ResourcesPath = "Resources");

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[] { "pt-BR" };
				options.SetDefaultCulture(supportedCultures[0]);
				options.AddSupportedCultures(supportedCultures);
				options.AddSupportedUICultures(supportedCultures);
			});

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					var erros = context.ModelState
						.Where(e => e.Value.Errors.Count > 0)
						.ToDictionary(
							e => e.Key,
							e => e.Value.Errors.Select(er => er.ErrorMessage).ToArray()
						);

					var resultado = ResultadoOperacao<object>.Falha(
						"Um ou mais erros de validação foram encontrados.",
						erros
					);

					return new BadRequestObjectResult(resultado);
				};
			});

			return services;
		}
	}

}
