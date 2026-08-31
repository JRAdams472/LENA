using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IRegionRepository : IAsyncRepository<Region>
    {
        Task<IReadOnlyList<Region>> GetAllByCountryIdAsync(int countryId, CancellationToken cancellationToken = default);
        Task<Region?> GetByNameAndCountryIdAsync(string name, int countryId, CancellationToken cancellationToken = default);
    }
}
