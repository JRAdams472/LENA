using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IBottleRepository : IAsyncRepository<Bottle>
    {
        Task<IReadOnlyList<Bottle>> GetAllByCountryIdAsync(int countryId);
        Task<IReadOnlyList<Bottle>> GetAllByRegionIdAsync(int regionId);
        Task<IReadOnlyList<Bottle>> GetAllByTypeIdAsync(int typeId);
        Task<IReadOnlyList<Bottle>> GetAllByVintageYearAsync(int vintageYear);
        Task<IReadOnlyList<Bottle>> GetFavoritesAsync();
        Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm);
        Task<int> GetTotalBottleCountAsync();
    }
}