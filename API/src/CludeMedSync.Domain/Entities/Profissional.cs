using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Domain.Models
{
	public class Profissional
	{
		public int Id { get; set; }
		public string NomeCompleto { get; set; }
		public string Especialidade { get; set; }
		public string CRM { get; set; }
		public string Email { get; set; }
		public string? Telefone { get; set; }
		public bool Ativo { get; set; } = true;

		public void Inativar() => Ativo = false;
	}
}
