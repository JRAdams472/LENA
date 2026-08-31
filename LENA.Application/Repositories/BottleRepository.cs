using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class BottleRepository : BaseRepository<Bottle>, IBottleRepository
    {
        public BottleRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByCountryIdAsync(int countryId, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByCountryId]", new { CountryId = countryId }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetAllByRegionIdAsync(int regionId, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByRegionId]", new { RegionId = regionId }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetAllByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByTypeId]", new { TypeId = typeId }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetAllByVintageYearAsync(int vintageYear, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByVintageYear]", new { VintageYear = vintageYear }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetFavoritesAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetFavorites]", cancellationToken: cancellationToken);

        public async Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_SearchBottles]", new { SearchTerm = searchTerm }, cancellationToken);

        public async Task<int> GetTotalBottleCountAsync(CancellationToken cancellationToken = default)
            => await QuerySingleAsync<int>("[Wine].[usp_Bottle_GetTotalBottleCount]", cancellationToken: cancellationToken);

        public override async Task<Bottle> CreateAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            entity.BottleID = await QuerySingleAsync<int>("[Wine].[usp_Bottle_Create]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Bottle?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Bottle>("[Wine].[usp_Bottle_GetById]", new { Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<Bottle>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_ListAll]", cancellationToken: cancellationToken);

        public override async Task<Bottle> UpdateAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Bottle_Update]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Bottle> DeleteAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Bottle_Delete]", new { BottleID = entity.BottleID }, cancellationToken);
            return entity;
        }

        public override async Task<Bottle?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Bottle>("[Wine].[usp_Bottle_GetByName]", new { Name = name }, cancellationToken);
    }
}
