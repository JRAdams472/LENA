using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFlavorProfileRepository : IAsyncRepository<FlavorProfile>
    {
        Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    }
}