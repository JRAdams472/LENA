using LENA.Application.Models;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IBottleRepository : IAsyncRepository<Bottle>, IGetByNameRepository<Bottle>
    {
        Task<IReadOnlyList<Bottle>> GetAllByCountryIdAsync(int countryId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Bottle>> GetAllByRegionIdAsync(int regionId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Bottle>> GetAllByTypeIdAsync(int typeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Bottle>> GetAllByVintageYearAsync(int vintageYear, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Bottle>> GetFavoritesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<int> GetTotalBottleCountAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<Bottle>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
