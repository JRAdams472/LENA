using LENA.Domain.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Contracts.Persistence
{
    public interface INutrientTypeRepository : IAsyncRepository<NutrientType>
    {
        Task<NutrientType?> GetByNameAsync(string name);
    }
}