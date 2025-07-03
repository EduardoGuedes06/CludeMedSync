using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Interfaces;
using Dapper;
using System.Data;

namespace CludeMedSync.Data.Repositories
{
	public abstract class Repository<T> : IRepository<T> where T : class
	{
		protected readonly DbConnectionFactory _connectionFactory;
		private readonly string _tableName;

		protected Repository(DbConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
			_tableName = $"{typeof(T).Name}";
		}

		protected IDbConnection CreateConnection() => _connectionFactory.CreateConnection();

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			using var connection = CreateConnection();

			var hasAtivo = typeof(T).GetProperty("Ativo") != null;
			var query = $"SELECT * FROM {_tableName}" + (hasAtivo ? " WHERE Ativo = 1" : "");

			return await connection.QueryAsync<T>(query);
		}

		public virtual async Task<T?> GetByIdAsync(int id)
		{
			using var connection = CreateConnection();

			var hasAtivo = typeof(T).GetProperty("Ativo") != null;
			var query = $"SELECT * FROM {_tableName} WHERE Id = @Id" + (hasAtivo ? " AND Ativo = 1" : "");

			return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
		}

		public virtual async Task<int> AddAsync(T entity)
		{
			var columns = GetColumns(excludeId: true);
			var columnNames = string.Join(", ", columns);
			var columnParams = string.Join(", ", columns.Select(c => $"@{c}"));

			var query = $"INSERT INTO {_tableName} ({columnNames}) VALUES ({columnParams}); SELECT LAST_INSERT_ID();";

			using var connection = CreateConnection();
			var newId = await connection.ExecuteScalarAsync<int>(query, entity);
			return newId;
		}

		public virtual async Task<bool> UpdateAsync(T entity)
		{
			var columns = GetColumns(excludeId: true);
			var setClause = string.Join(", ", columns.Select(c => $"{c} = @{c}"));
			var idProperty = entity.GetType().GetProperty("Id")?.GetValue(entity);

			var query = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";

			using var connection = CreateConnection();
			var affectedRows = await connection.ExecuteAsync(query, entity);
			return affectedRows > 0;
		}

		public virtual async Task<bool> DeleteAsync(int id)
		{
			using var connection = CreateConnection();
			if (typeof(T).GetProperty("Ativo") != null)
			{
				var query = $"UPDATE {_tableName} SET Ativo = 0 WHERE Id = @Id";
				var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
				return affectedRows > 0;
			}
			else
			{
				var query = $"DELETE FROM {_tableName} WHERE Id = @Id";
				var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
				return affectedRows > 0;
			}
		}

		private IEnumerable<string> GetColumns(bool excludeId = false)
		{
			var properties = typeof(T).GetProperties().Where(p => p.Name != "Id" || !excludeId);
			return properties.Select(p => p.Name);
		}
	}
}
