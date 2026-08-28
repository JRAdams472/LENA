using LENA.Domain.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFlavorProfileRepository : IAsyncRepository<FlavorProfile>
    {
        Task<FlavorProfile> GetByNameAsync(string name);
        Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync();
    }
}