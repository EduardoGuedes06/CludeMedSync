using CludeMedSync.Data.Context;
using CludeMedSync.Data.Repositories;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Service.Interfaces;
using CludeMedSync.Service.Services;

namespace CludeMedSync.Api.Configuration
{
	public static class DependencyInjectionConfig
	{
		public static IServiceCollection ResolveDependencies(this IServiceCollection services)
		{
			services.AddSingleton<DbConnectionFactory>();
			services.AddScoped<IPacienteRepository, PacienteRepository>();
			services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
			services.AddScoped<IConsultaRepository, ConsultaRepository>();

			services.AddScoped<IPacienteService, PacienteService>();
			services.AddScoped<IProfissionalService, ProfissionalService>();
			services.AddScoped<IConsultaService, ConsultaService>();

			return services;
		}
	}
}
