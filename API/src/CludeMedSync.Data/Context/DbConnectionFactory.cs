using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace CludeMedSync.Data.Context
{
	public class DbConnectionFactory
	{
		private readonly string _connectionString;

		public DbConnectionFactory(Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("connection")
				?? throw new InvalidOperationException("Connection string 'connection' not found.");
		}

		public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
	}
}
