using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class FoodNutrientRepository : BaseRepository<FoodNutrient>, IFoodNutrientRepository
    {
        public FoodNutrientRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<FoodNutrient>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<FoodNutrient>("[Inventory].[usp_FoodNutrient_ListAll]", cancellationToken: cancellationToken);

        public override async Task<FoodNutrient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<FoodNutrient>("[Inventory].[usp_FoodNutrient_GetById]", new { Id = id }, cancellationToken);

        public override async Task<FoodNutrient?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await Task.FromResult<FoodNutrient?>(null);

        public override async Task<FoodNutrient> CreateAsync(FoodNutrient entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Inventory].[usp_FoodNutrient_Create]", new
            {
                entity.FoodId,
                entity.NutrientId,
                entity.AmountPerServing
            }, cancellationToken);
            return entity;
        }

        public override async Task<FoodNutrient> UpdateAsync(FoodNutrient entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_FoodNutrient_Update]", new
            {
                entity.FoodId,
                entity.NutrientId,
                entity.AmountPerServing
            }, nameof(FoodNutrient), new { entity.FoodId, entity.NutrientId }, cancellationToken);
            return entity;
        }

        public override async Task<FoodNutrient> DeleteAsync(FoodNutrient entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Inventory].[usp_FoodNutrient_Delete]", new { entity.FoodId, entity.NutrientId }, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<FoodNutrient>> GetByFoodIdAsync(int foodId, CancellationToken cancellationToken = default)
            => await QueryListAsync<FoodNutrient>("[Inventory].[usp_FoodNutrient_GetByFoodId]", new { FoodId = foodId }, cancellationToken);

        public async Task<IEnumerable<FoodNutrient>> GetByNutrientIdAsync(int nutrientId, CancellationToken cancellationToken = default)
            => await QueryListAsync<FoodNutrient>("[Inventory].[usp_FoodNutrient_GetByNutrientId]", new { NutrientId = nutrientId }, cancellationToken);

        public async Task<FoodNutrient?> GetByFoodAndNutrientIdAsync(int foodId, int nutrientId, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<FoodNutrient>("[Inventory].[usp_FoodNutrient_GetByFoodAndNutrientId]", new { FoodId = foodId, NutrientId = nutrientId }, cancellationToken);
    }
}
