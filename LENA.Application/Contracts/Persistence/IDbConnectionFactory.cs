using System.Data.Common;

namespace LENA.Application.Contracts.Persistence
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
    }
}
