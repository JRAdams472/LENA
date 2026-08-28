using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFlavorProfileRepository : IAsyncRepository<FlavorProfile>
    {
        Task<FlavorProfile> GetByNameAsync(string name);
        Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync();
    }
}