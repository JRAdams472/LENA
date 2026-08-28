using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Application.Contracts.Persistence
{
    public interface ICountryRepository : IWineRepository<Country>
    {
        Task<Country> GetByISOCodeAsync(string isoCode);
        Task<IReadOnlyList<Country>> GetAllActiveAsync();
    }
}