using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LENA.Application.Contracts.Persistence
{
    public interface IDbConnectionFactory
    {
        Task<SqlConnection> CreateConnectionAsync();
    }
}