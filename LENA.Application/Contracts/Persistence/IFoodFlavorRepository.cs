using LENA.Domain.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFoodFlavorRepository : IAsyncRepository<FoodFlavor>
    {
        Task<IEnumerable<FoodFlavor>> GetByFoodIdAsync(int foodId);
        Task<IEnumerable<FoodFlavor>> GetByFlavorIdAsync(int flavorId);
        Task<FoodFlavor> GetByFoodAndFlavorIdAsync(int foodId, int flavorId);
    }
}