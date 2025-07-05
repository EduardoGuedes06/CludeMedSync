using CludeMedSync.Data.Auth;
using CludeMedSync.Data.Context;
using CludeMedSync.Data.Repositories;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using CludeMedSync.Service.Interfaces;
using CludeMedSync.Service.Services;
using Microsoft.AspNetCore.Identity;

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
			services.AddScoped<IConsultaLogRepository, ConsultaLogRepository>();

			services.AddScoped<IUserStore<Usuario>, DapperUserStore>();

			services.AddScoped<IPacienteService, PacienteService>();
			services.AddScoped<IProfissionalService, ProfissionalService>();
			services.AddScoped<IConsultaService, ConsultaService>();

			services.AddScoped<IAuthService, AuthService>();

			return services;
		}
	}
}
