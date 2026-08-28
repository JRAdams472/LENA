using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFoodNutrientRepository : IAsyncRepository<FoodNutrient>
    {
        Task<IEnumerable<FoodNutrient>> GetByFoodIdAsync(int foodId);
        Task<IEnumerable<FoodNutrient>> GetByNutrientIdAsync(int nutrientId);
        Task<FoodNutrient> GetByFoodAndNutrientIdAsync(int foodId, int nutrientId);
    }
}