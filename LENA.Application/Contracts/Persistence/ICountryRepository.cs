using LENA.Application.Models;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface ICountryRepository : IAsyncRepository<Country>
    {
        Task<Country?> GetByISOCodeAsync(string isoCode, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Country>> GetAllActiveAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<Country>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
