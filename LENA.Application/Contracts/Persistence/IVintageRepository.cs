using LENA.Application.Models;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IVintageRepository : IAsyncRepository<Vintage>
    {
        Task<Vintage?> GetByYearAsync(int year, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Vintage>> GetAllActiveAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<Vintage>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
