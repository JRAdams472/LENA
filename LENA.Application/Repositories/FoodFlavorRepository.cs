using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class FoodFlavorRepository : BaseRepository<FoodFlavor>, IFoodFlavorRepository
    {
        public FoodFlavorRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<LENA.Application.Models.PagedResult<FoodFlavor>> ListAllAsync(LENA.Application.Models.PaginationRequest? paging = null, CancellationToken cancellationToken = default)
            => await QueryPagedAsync<FoodFlavor>("[Inventory].[usp_FoodFlavor_ListAll]", paging, cancellationToken);

        public override async Task<FoodFlavor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<FoodFlavor>("[Inventory].[usp_FoodFlavor_GetById]", new { Id = id }, cancellationToken);

        public override Task<FoodFlavor?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => Task.FromResult<FoodFlavor?>(null);

        public override async Task<FoodFlavor> CreateAsync(FoodFlavor entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Inventory].[usp_FoodFlavor_Create]", new
            {
                entity.FoodId,
                entity.FlavorId,
                entity.IntensityScore
            }, cancellationToken);
            return entity;
        }

        public override async Task<FoodFlavor> UpdateAsync(FoodFlavor entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_FoodFlavor_Update]", new
            {
                entity.FoodId,
                entity.FlavorId,
                entity.IntensityScore
            }, nameof(FoodFlavor), new { entity.FoodId, entity.FlavorId }, cancellationToken);
            return entity;
        }

        public override async Task<FoodFlavor> DeleteAsync(FoodFlavor entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_FoodFlavor_Delete]", new { entity.FoodId, entity.FlavorId }, nameof(FoodFlavor), $"{entity.FoodId}-{entity.FlavorId}", cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<FoodFlavor>> GetByFoodIdAsync(int foodId, CancellationToken cancellationToken = default)
            => await QueryListAsync<FoodFlavor>("[Inventory].[usp_FoodFlavor_GetByFoodId]", new { FoodId = foodId }, cancellationToken);

        public async Task<IEnumerable<FoodFlavor>> GetByFlavorIdAsync(int flavorId, CancellationToken cancellationToken = default)
            => await QueryListAsync<FoodFlavor>("[Inventory].[usp_FoodFlavor_GetByFlavorId]", new { FlavorId = flavorId }, cancellationToken);

        public async Task<FoodFlavor?> GetByFoodAndFlavorIdAsync(int foodId, int flavorId, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<FoodFlavor>("[Inventory].[usp_FoodFlavor_GetByFoodAndFlavorId]", new { FoodId = foodId, FlavorId = flavorId }, cancellationToken);
    }
}
