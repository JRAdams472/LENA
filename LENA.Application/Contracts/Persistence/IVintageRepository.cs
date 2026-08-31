using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface IVintageRepository : IAsyncRepository<Vintage>
    {
        Task<Vintage?> GetByYearAsync(int year);
        Task<IReadOnlyList<Vintage>> GetAllActiveAsync();
    }
}