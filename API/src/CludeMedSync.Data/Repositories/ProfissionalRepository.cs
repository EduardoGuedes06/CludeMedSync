using CludeMedSync.Data.Context;
using CludeMedSync.Data.Repositories.Utils;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using Dapper;

namespace CludeMedSync.Data.Repositories
{
	public class ProfissionalRepository : Repository<Profissional>, IProfissionalRepository
	{
		public ProfissionalRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		public async Task<(bool existeConflito, string mensagem)> VerificarDuplicidadeProfissionalAsync(string crm, string email, string? telefone)
		{
			using var connection = CreateConnection();

			var mensagens = new List<string>();
			var existeConflito = false;

			var crmQuery = $"SELECT 1 FROM {nameof(Profissional)} WHERE {nameof(Profissional.CRM)} = '{crm}' AND Ativo = 1 LIMIT 1";
			if (await connection.ExecuteScalarAsync<int?>(crmQuery) != null)
			{
				mensagens.Add($"Já existe um profissional cadastrado com este {nameof(Profissional.CRM)}.");
				existeConflito = true;
			}
			var emailQuery = $"SELECT 1 FROM {nameof(Profissional)} WHERE {nameof(Profissional.Email)} = '{email}' AND Ativo = 1 LIMIT 1";
			if (await connection.ExecuteScalarAsync<int?>(emailQuery) != null)
			{
				mensagens.Add($"Já existe um {nameof(Profissional)} cadastrado com este {nameof(Profissional.Email)}.");
				existeConflito = true;
			}
			if (!string.IsNullOrWhiteSpace(telefone))
			{
				var telQuery = $"SELECT 1 FROM {nameof(Profissional)} WHERE {nameof(Profissional.Telefone)} = '{telefone}' AND Ativo = 1 LIMIT 1";
				if (await connection.ExecuteScalarAsync<int?>(telQuery) != null)
				{
					mensagens.Add($"Já existe um {nameof(Profissional.CRM)} cadastrado com este Telefone.");
					existeConflito = true;
				}
			}

			return (existeConflito, string.Join("\n", mensagens));
		}



	}
}
