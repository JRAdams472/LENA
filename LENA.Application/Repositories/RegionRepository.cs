using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class RegionRepository : BaseRepository<Region>, IRegionRepository
    {
        public RegionRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<Region>> GetAllByCountryIdAsync(int countryId, CancellationToken cancellationToken = default)
            => await QueryListAsync<Region>("[Wine].[usp_Region_GetAllByCountryId]", new { CountryId = countryId }, cancellationToken);

        public async Task<Region?> GetByNameAndCountryIdAsync(string name, int countryId, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Region>("[Wine].[usp_Region_GetByNameAndCountryId]", new { Name = name, CountryId = countryId }, cancellationToken);

        public override async Task<Region> CreateAsync(Region entity, CancellationToken cancellationToken = default)
        {
            entity.RegionID = await QuerySingleAsync<int>("[Wine].[usp_Region_Create]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Region?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Region>("[Wine].[usp_Region_GetById]", new { Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<Region>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Region>("[Wine].[usp_Region_ListAll]", cancellationToken: cancellationToken);

        public override async Task<Region> UpdateAsync(Region entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Region_Update]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Region> DeleteAsync(Region entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Region_Delete]", new { RegionID = entity.RegionID }, cancellationToken);
            return entity;
        }

        public override async Task<Region?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Region>("[Wine].[usp_Region_GetByName]", new { Name = name }, cancellationToken);
    }
}
