using LENA.Domain.Entity.Wine;

namespace LENA.Application.Contracts.Persistence
{
    public interface ICountryRepository : IWineRepository<Country>
    {
        Task<Country?> GetByISOCodeAsync(string isoCode);
        Task<IReadOnlyList<Country>> GetAllActiveAsync();
    }
}