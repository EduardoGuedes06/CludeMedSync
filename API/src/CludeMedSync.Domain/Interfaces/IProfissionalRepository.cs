using CludeMedSync.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Interfaces
{
	public interface IProfissionalRepository : IRepository<Profissional>
	{
		Task<(bool existeConflito, string mensagem)> VerificarDuplicidadeProfissionalAsync(string crm, string email, string? telefone);
	}
}
