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
		Task<Paciente?> GetByCpfAsync(string cpf);
	}
}
