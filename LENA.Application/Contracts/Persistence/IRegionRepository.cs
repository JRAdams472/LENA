using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IRegionRepository : IWineRepository<Region>
    {
        Task<IReadOnlyList<Region>> GetAllByCountryIdAsync(int countryId);
        Task<Region?> GetByNameAndCountryIdAsync(string name, int countryId);
    }
}