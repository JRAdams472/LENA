using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Application.Contracts.Persistence
{
    public interface IWineRepository<T> : IAsyncRepository<T> where T : class
    {
        Task<T> GetByNameAsync(string name);
        Task<IReadOnlyList<T>> GetAllByCountryIdAsync(int countryId);
        Task<IReadOnlyList<T>> GetAllByRegionIdAsync(int regionId);
        Task<IReadOnlyList<T>> GetAllByTypeIdAsync(int typeId);
        Task<IReadOnlyList<T>> GetAllByVintageYearAsync(int vintageYear);
    }
}