using LENA.Application.Models;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Contracts.Persistence
{
    public interface ITypeRepository : IAsyncRepository<TypeEntity>
    {
        Task<PagedResult<TypeEntity>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
