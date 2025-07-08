using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Domain.Interfaces;
using Dapper;
using System.Data;

namespace CludeMedSync.Data.Repositories.Utils
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

		public virtual async Task<PagedResult<T>> GetPaginadoComplexoAsync<T>(
			int page,
			int pageSize,
			string colunasSelect,
			string tabelasFromJoin,
			string? whereFixo = null,
			object? filtrosDinamicos = null,
			string? orderBy = null,
			bool orderByDesc = false)
				{
					using var connection = _connectionFactory.CreateConnection();

					int currentPage = page > 0 ? page : 1;
					int offset = (currentPage - 1) * pageSize;
					string order = orderByDesc ? "DESC" : "ASC";
					string orderByColumn = !string.IsNullOrWhiteSpace(orderBy) ? orderBy : "(SELECT NULL)";

					var conditions = new List<string>();
					if (!string.IsNullOrWhiteSpace(whereFixo))
					{
						conditions.Add(whereFixo);
					}

					var parameters = new DynamicParameters();
					if (filtrosDinamicos != null)
					{
						var props = filtrosDinamicos.GetType().GetProperties();
						foreach (var prop in props)
						{
							conditions.Add($"{prop.Name} = @{prop.Name}");
							parameters.Add(prop.Name, prop.GetValue(filtrosDinamicos));
						}
					}

					string whereClause = conditions.Any() ? "WHERE " + string.Join(" AND ", conditions) : "";

					string sqlCount = $"SELECT COUNT(*) FROM {tabelasFromJoin} {whereClause};";

					string sqlQuery = $@"
								SELECT {colunasSelect}
								FROM {tabelasFromJoin}
								{whereClause}
								ORDER BY {orderByColumn} {order}
								LIMIT @PageSize OFFSET @Offset;
							";

					parameters.Add("PageSize", pageSize);
					parameters.Add("Offset", offset);

					using var multi = await connection.QueryMultipleAsync(sqlCount + sqlQuery, parameters);

					int totalCount = await multi.ReadFirstAsync<int>();
					var items = (await multi.ReadAsync<T>()).ToList();

					return new PagedResult<T>(items, totalCount, currentPage, pageSize);
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
