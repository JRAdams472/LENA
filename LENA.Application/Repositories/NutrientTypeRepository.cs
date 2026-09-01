using LENA.Application.Contracts.Persistence;
using LENA.Application.Models;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class NutrientTypeRepository : BaseRepository<NutrientType>, INutrientTypeRepository
    {
        public NutrientTypeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<NutrientType>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<NutrientType>("[Inventory].[usp_NutrientType_ListAll]", cancellationToken: cancellationToken);

        public override async Task<NutrientType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<NutrientType>("[Inventory].[usp_NutrientType_GetById]", new { Id = id }, cancellationToken);

        public async Task<NutrientType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<NutrientType>("[Inventory].[usp_NutrientType_GetByName]", new { Name = name }, cancellationToken);

        public override async Task<NutrientType> CreateAsync(NutrientType entity, CancellationToken cancellationToken = default)
        {
            entity.NutrientId = await QuerySingleAsync<int>("[Inventory].[usp_NutrientType_Create]", new
            {
                entity.NutrientName,
                entity.UnitOfMeasure
            }, cancellationToken);
            return entity;
        }

        public override async Task<NutrientType> UpdateAsync(NutrientType entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_NutrientType_Update]", new
            {
                entity.NutrientId,
                entity.NutrientName,
                entity.UnitOfMeasure
            }, nameof(NutrientType), entity.NutrientId, cancellationToken);
            return entity;
        }

        public override async Task<NutrientType> DeleteAsync(NutrientType entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_NutrientType_Delete]", new { entity.NutrientId }, nameof(NutrientType), entity.NutrientId, cancellationToken);
            return entity;
        }
    }
}
