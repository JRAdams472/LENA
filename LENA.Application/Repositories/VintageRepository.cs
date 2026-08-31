using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class VintageRepository : BaseRepository<Vintage>, IVintageRepository
    {
        public VintageRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<Vintage> CreateAsync(Vintage entity, CancellationToken cancellationToken = default)
        {
            entity.VintageID = await QuerySingleAsync<int>("[Wine].[usp_Vintage_Create]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Vintage?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Vintage>("[Wine].[usp_Vintage_GetById]", new { Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<Vintage>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Vintage>("[Wine].[usp_Vintage_ListAll]", cancellationToken: cancellationToken);

        public override async Task<Vintage> UpdateAsync(Vintage entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Vintage_Update]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Vintage> DeleteAsync(Vintage entitey, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Vintage_Delete]", new { entitey.VintageID }, cancellationToken);
            return entitey;
        }

        public override async Task<Vintage?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await Task.FromResult<Vintage?>(null);

        public async Task<Vintage?> GetByYearAsync(int year, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Vintage>("[Wine].[usp_Vintage_GetByYear]", new { Year = year }, cancellationToken);

        public async Task<IReadOnlyList<Vintage>> GetAllActiveAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Vintage>("[Wine].[usp_Vintage_GetAllActive]", cancellationToken: cancellationToken);
    }
}
