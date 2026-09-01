using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFlavorProfileRepository : IAsyncRepository<FlavorProfile>, IGetByNameRepository<FlavorProfile>
    {
        Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    }
}
