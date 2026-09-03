using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class BottleRepository : BaseRepository<Bottle>, IBottleRepository
    {
        private readonly ICurrentUserService _currentUser;
        private readonly TimeProvider _timeProvider;

        public BottleRepository(IDbConnectionFactory connectionFactory, ICurrentUserService currentUser, TimeProvider? timeProvider = null) : base(connectionFactory)
        {
            _currentUser = currentUser;
            _timeProvider = timeProvider ?? TimeProvider.System;
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByCountryIdAsync(int countryId, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByCountryId]", new { UserID = _currentUser.UserID, CountryId = countryId }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetAllByRegionIdAsync(int regionId, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByRegionId]", new { UserID = _currentUser.UserID, RegionId = regionId }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetAllByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByTypeId]", new { UserID = _currentUser.UserID, TypeId = typeId }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetAllByVintageYearAsync(int vintageYear, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetAllByVintageYear]", new { UserID = _currentUser.UserID, VintageYear = vintageYear }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> GetFavoritesAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_GetFavorites]", new { UserID = _currentUser.UserID }, cancellationToken);

        public async Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm, CancellationToken cancellationToken = default)
            => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_SearchBottles]", new { UserID = _currentUser.UserID, SearchTerm = searchTerm }, cancellationToken);

        public async Task<int> GetTotalBottleCountAsync(CancellationToken cancellationToken = default)
            => await QuerySingleAsync<int>("[Wine].[usp_Bottle_GetTotalBottleCount]", new { UserID = _currentUser.UserID }, cancellationToken: cancellationToken);

        public async Task SetFavoriteAsync(int bottleId, bool isFavorite, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Wine].[usp_Bottle_SetFavorite]", new
            {
                UserID = _currentUser.UserID,
                BottleID = bottleId,
                IsFavorite = isFavorite,
                CreatedBy = _currentUser.UserName,
                CreateDate = _timeProvider.GetUtcNow().UtcDateTime,
                LastUpdatedBy = _currentUser.UserName,
                LastUpdatedDate = _timeProvider.GetUtcNow().UtcDateTime
            }, cancellationToken);

        public override async Task<Bottle> CreateAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            entity.BottleID = await QuerySingleAsync<int>("[Wine].[usp_Bottle_Create]", new
            {
                UserID = _currentUser.UserID,
                entity.BottleNumber,
                entity.TypeID,
                entity.CountryID,
                entity.RegionID,
                entity.VintageYear,
                entity.Vineyard,
                entity.ABV,
                entity.Acidity,
                entity.TanninLevel,
                entity.Body,
                entity.Sweetness,
                entity.OakIntegration,
                entity.BottleSize,
                entity.Quantity,
                entity.PurchaseDate,
                entity.PurchasePrice,
                entity.StorageTemp,
                entity.Location,
                entity.Notes,
                entity.IsFavorite,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<Bottle?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Bottle>("[Wine].[usp_Bottle_GetById]", new { UserID = _currentUser.UserID, Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<Bottle>> ListAllAsync(CancellationToken cancellationToken = default)
        => await QueryListAsync<Bottle>("[Wine].[usp_Bottle_ListAll]", new { UserID = _currentUser.UserID }, cancellationToken: cancellationToken);

        public async Task<LENA.Application.Models.PagedResult<Bottle>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
            => await QueryPagedListAsync<Bottle>("[Wine].[usp_Bottle_ListAllPaged]", pageNumber, pageSize, new { UserID = _currentUser.UserID }, ct: ct);

        public override async Task<Bottle> UpdateAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Bottle_Update]", new
            {
                UserID = _currentUser.UserID,
                entity.BottleID,
                entity.BottleNumber,
                entity.TypeID,
                entity.CountryID,
                entity.RegionID,
                entity.VintageYear,
                entity.Vineyard,
                entity.ABV,
                entity.Acidity,
                entity.TanninLevel,
                entity.Body,
                entity.Sweetness,
                entity.OakIntegration,
                entity.BottleSize,
                entity.Quantity,
                entity.PurchaseDate,
                entity.PurchasePrice,
                entity.StorageTemp,
                entity.Location,
                entity.Notes,
                entity.IsFavorite,
                entity.LastUpdatedBy,
                entity.LastUpdatedDate
            }, nameof(Bottle), entity.BottleID, cancellationToken);
            return entity;
        }

        public override async Task<Bottle> DeleteAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Bottle_Delete]", new { UserID = _currentUser.UserID, BottleID = entity.BottleID }, nameof(Bottle), entity.BottleID, cancellationToken);
            return entity;
        }

        public async Task<Bottle?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Bottle>("[Wine].[usp_Bottle_GetByName]", new { UserID = _currentUser.UserID, Name = name }, cancellationToken);
    }
}
