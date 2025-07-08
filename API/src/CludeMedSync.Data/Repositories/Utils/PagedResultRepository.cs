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

			string whereClause = "";
			if (filtros != null)
			{
				var props = filtros.GetType().GetProperties();
				var conditions = props.Select(p => $"{p.Name} = @{p.Name}");
				whereClause = "WHERE " + string.Join(" AND ", conditions);
			}

			var countSql = $"SELECT COUNT(*) FROM {_tableName} {whereClause};";

			var dataSql = $@"
				SELECT * FROM {_tableName}
				{whereClause}
				ORDER BY {orderByColumn} {order}
				LIMIT @PageSize OFFSET @Offset;
			";

			var parameters = new DynamicParameters();
			parameters.Add("Offset", offset);
			parameters.Add("PageSize", pageSize);

			if (filtros != null)
			{
				foreach (var prop in filtros.GetType().GetProperties())
				{
					parameters.Add(prop.Name, prop.GetValue(filtros));
				}
			}

			int totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);
			var items = (await connection.QueryAsync<T>(dataSql, parameters)).ToList();

			return new PagedResult<T>(items, totalCount, currentPage, pageSize);
		}


	}
}
