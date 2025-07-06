using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Entities.Utils.Enums
{
	public enum EnumStatusConsulta
	{
		Agendada = 1,
		Confirmada = 2,
		EmAndamento = 3,
		Realizada = 4,
		Cancelada = 5,
		PacienteNaoCompareceu = 6,
		ProfissionalNaoCompareceu = 7
	}
}
