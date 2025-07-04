using CludeMedSync.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Interfaces
{
	public interface IPacienteRepository : IRepository<Paciente>
	{
		Task<(bool existeConflito, string mensagem)> VerificarDuplicidadePacienteAsync(string cpf, string email, string? telefone);
	}
}
