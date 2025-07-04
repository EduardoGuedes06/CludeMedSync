using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CludeMedSync.Service.Common
{
	public class ResultadoOperacao<T>
	{
		public bool Sucesso { get; set; }
		public string Mensagem { get; set; }
		public int? Status { get; set; }
		public T? Dados { get; set; }

		public static ResultadoOperacao<T> Ok(string mensagem, T? dados = default)
		{
			return new ResultadoOperacao<T>
			{
				Sucesso = true,
				Mensagem = mensagem,
				Dados = dados,
				Status = 200
			};
		}

		public static ResultadoOperacao<T> Falha(string mensagem, T? dados = default)
		{
			return new ResultadoOperacao<T>
			{
				Sucesso = false,
				Mensagem = mensagem,
				Dados = dados,
				Status = 400
			};
		}
	}
}
