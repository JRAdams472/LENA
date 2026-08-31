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

        public abstract Task<T> CreateAsync(T entity);
        public abstract Task<T?> GetByIdAsync(int Id);
        public abstract Task<T?> GetByNameAsync(string Name);
        public abstract Task<T> UpdateAsync(T entity);
        public abstract Task<T> DeleteAsync(T entitey);
        public abstract Task<IReadOnlyList<T>> ListAllAsync();
    }
}