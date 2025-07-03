using CludeMedSync.Service.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CludeMedSync.Api.Configuration
{
	public static class MvcConfig
	{
		public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
		{
			services.AddControllers()
				.AddDataAnnotationsLocalization();
			services.AddFluentValidationAutoValidation();
			services.AddValidatorsFromAssemblyContaining<CreatePacienteDtoValidator>();
			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[] { "pt-BR" };
				options.SetDefaultCulture(supportedCultures[0]);
				options.AddSupportedCultures(supportedCultures);
				options.AddSupportedUICultures(supportedCultures);
			});

			return services;
		}
	}
}
