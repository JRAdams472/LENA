using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Application.Contracts.Persistence
{
    public interface IVintageRepository : IWineRepository<Vintage>
    {
        Task<Vintage> GetByYearAsync(int year);
        Task<IReadOnlyList<Vintage>> GetAllActiveAsync();
    }
}