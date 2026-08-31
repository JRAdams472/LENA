using Microsoft.Data.SqlClient;

namespace LENA.Application.Contracts.Persistence
{
    public interface IDbConnectionFactory
    {
        Task<SqlConnection> CreateConnectionAsync();
    }
}