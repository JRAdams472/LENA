using LENA.Application.Contracts.Persistence;

namespace LENA.Application.Repositories
{
    public abstract class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly IDbConnectionFactory _connectionFactory;

        protected BaseRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public abstract Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public abstract Task<T?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        public abstract Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
    }
}