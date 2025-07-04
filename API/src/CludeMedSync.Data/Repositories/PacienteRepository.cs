using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Interfaces;
using CludeMedSync.Domain.Models;
using Dapper;

namespace CludeMedSync.Data.Repositories
{
	public class PacienteRepository : Repository<Paciente>, IPacienteRepository
	{
		public PacienteRepository(DbConnectionFactory connectionFactory) : base(connectionFactory)
		{
		}

		public async Task<(bool existeConflito, string mensagem)> VerificarDuplicidadePacienteAsync(string cpf, string email, string? telefone)
		{
			using var connection = CreateConnection();

			var mensagens = new List<string>();
			var existeConflito = false;

			var cpfQuery = $"SELECT 1 FROM {nameof(Paciente)} WHERE {nameof(Paciente.CPF)} = '{cpf}' AND Ativo = 1 LIMIT 1";
			if (await connection.ExecuteScalarAsync<int?>(cpfQuery) != null)
			{
				mensagens.Add($"Já existe um {nameof(Paciente)} cadastrado com este {nameof(Paciente.CPF)}.");
				existeConflito = true;
			}

			var emailQuery = $"SELECT 1 FROM {nameof(Paciente)} WHERE {nameof(Paciente.Email)} = '{email}' AND Ativo = 1 LIMIT 1";
			if (await connection.ExecuteScalarAsync<int?>(emailQuery) != null)
			{
				mensagens.Add($"Já existe um {nameof(Paciente)} cadastrado com este {nameof(Paciente.Email)}.");
				existeConflito = true;
			}

			if (!string.IsNullOrWhiteSpace(telefone))
			{
				var telQuery = $"SELECT 1 FROM {nameof(Paciente)} WHERE {nameof(Paciente.Telefone)} = '{telefone}' AND Ativo = 1 LIMIT 1";
				if (await connection.ExecuteScalarAsync<int?>(telQuery) != null)
				{
					mensagens.Add($"Já existe um {nameof(Paciente)} cadastrado com este {nameof(Paciente.Telefone)}.");
					existeConflito = true;
				}
			}

			return (existeConflito, string.Join("  \n", mensagens));
		}



	}
}
