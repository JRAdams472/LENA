using System.Data.Common;
using Dapper;
using LENA.Application.Contracts.Persistence;
using Microsoft.Data.SqlClient;

namespace LENA.Infrastructure.Persistence
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        static DbConnectionFactory()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = new SqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync(cancellationToken);
                return connection;
            }
            catch
            {
                await connection.DisposeAsync();
                throw;
            }
        }
    }
}
