using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFlavorProfileRepository : IAsyncRepository<FlavorProfile>
    {
        Task<FlavorProfile> GetByNameAsync(string name);
        Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync();
    }
}