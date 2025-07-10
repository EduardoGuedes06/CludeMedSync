using CludeMedSync.Data.Context;
using CludeMedSync.Domain.Entities.Pagination;
using CludeMedSync.Domain.Entities.Utils;
using CludeMedSync.Domain.Interfaces;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace CludeMedSync.Data.Repositories.Utils
{
	public class PagedResultRepository<T> : IPagedResultRepository<T> where T : class
	{
		protected readonly DbConnectionFactory _connectionFactory;
		private readonly string _tableName;

		public PagedResultRepository(DbConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
			_tableName = typeof(T).Name;
		}

		public async Task<PagedResult<T>> ObterPaginadoGenericoAsync(
		int page,
		int pageSize,
		object? filtros = null,
		string? orderBy = null,
		bool? ativo = null,
		bool orderByDesc = false)
		{
			using var connection = _connectionFactory.CreateConnection();

			int currentPage = page > 0 ? page : 1;
			int offset = (currentPage - 1) * pageSize;
			string order = orderByDesc ? "DESC" : "ASC";
			string orderByColumn = !string.IsNullOrWhiteSpace(orderBy) ? orderBy : "Id";

			var whereParts = new List<string>();
			var parameters = new DynamicParameters();

			parameters.Add("Offset", offset);
			parameters.Add("PageSize", pageSize);

			if (filtros is IDictionary<string, object> filtroDict && filtroDict.Any())
			{
				foreach (var kvp in filtroDict)
				{
					var campo = kvp.Key;
					var valor = kvp.Value;

					var lowerCampo = campo.ToLowerInvariant();
					bool usarLike = lowerCampo.Contains("Nome") || lowerCampo.Contains("Email") || lowerCampo.Contains("Telefone");

					if (usarLike)
					{
						whereParts.Add($"{campo} LIKE @{campo}");
						parameters.Add(campo, $"%{valor}%");
					}
					else
					{
						whereParts.Add($"{campo} = @{campo}");
						parameters.Add(campo, valor);
					}
				}
			}

			string whereClause = whereParts.Any() ? "WHERE " + string.Join(" AND ", whereParts) : "";

			string countSql = $"SELECT COUNT(*) FROM {_tableName} {whereClause};";
			string dataSql = $@"
				SELECT * FROM {_tableName}
				{whereClause}
				ORDER BY {orderByColumn} {order}
				LIMIT @PageSize OFFSET @Offset;
			";

			int totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);
			var items = (await connection.QueryAsync<T>(dataSql, parameters)).ToList();

			return new PagedResult<T>(items, totalCount, currentPage, pageSize);
		}


	}
}
