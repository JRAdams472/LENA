using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class FlavorProfileRepository : BaseRepository<FlavorProfile>, IFlavorProfileRepository
    {
        public FlavorProfileRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<FlavorProfile>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<FlavorProfile>("[Inventory].[usp_FlavorProfile_ListAll]", cancellationToken: cancellationToken);

        public override async Task<FlavorProfile?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<FlavorProfile>("[Inventory].[usp_FlavorProfile_GetById]", new { Id = id }, cancellationToken);

        public override async Task<FlavorProfile?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<FlavorProfile>("[Inventory].[usp_FlavorProfile_GetByName]", new { Name = name }, cancellationToken);

        public override async Task<FlavorProfile> CreateAsync(FlavorProfile entity, CancellationToken cancellationToken = default)
        {
            entity.FlavorId = await QuerySingleAsync<int>("[Inventory].[usp_FlavorProfile_Create]", entity, cancellationToken);
            return entity;
        }

        public override async Task<FlavorProfile> UpdateAsync(FlavorProfile entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Inventory].[usp_FlavorProfile_Update]", entity, cancellationToken);
            return entity;
        }

        public override async Task<FlavorProfile> DeleteAsync(FlavorProfile entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Inventory].[usp_FlavorProfile_Delete]", new { entity.FlavorId }, cancellationToken);
            return entity;
        }

        public async Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<FlavorProfile>("[Inventory].[usp_FlavorProfile_GetAllActive]", cancellationToken: cancellationToken);
    }
}
