using CludeMedSync.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Interfaces
{
	public interface IConsultaRepository : IRepository<Consulta>
	{
		Task<bool> ExisteConsultaNoMesmoDiaAsync(int pacienteId, int profissionalId, DateTime data);
		Task<bool> ExisteConsultaNoMesmoHorarioAsync(int profissionalId, DateTime dataHoraInicio);
	}
}
