using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface ICountryRepository : IAsyncRepository<Country>
    {
        Task<Country?> GetByISOCodeAsync(string isoCode, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Country>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    }
}