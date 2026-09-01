using LENA.Application.Models;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IRegionRepository : IAsyncRepository<Region>
    {
        Task<IReadOnlyList<Region>> GetAllByCountryIdAsync(int countryId, CancellationToken cancellationToken = default);
        Task<Region?> GetByNameAndCountryIdAsync(string name, int countryId, CancellationToken cancellationToken = default);

        Task<PagedResult<Region>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
