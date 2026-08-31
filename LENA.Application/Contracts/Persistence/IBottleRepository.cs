using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IBottleRepository : IWineRepository<Bottle>
    {
        Task<IReadOnlyList<Bottle>> GetFavoritesAsync();
        Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm);
        Task<int> GetTotalBottleCountAsync();
    }
}