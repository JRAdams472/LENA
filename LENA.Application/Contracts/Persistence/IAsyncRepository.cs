using LENA.Application.Models;

namespace LENA.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<PagedResult<T>> ListAllAsync(PaginationRequest? paging = null, CancellationToken cancellationToken = default);
    }
}
