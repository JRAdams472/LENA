using Dapper;

using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Infrastructure.Persistence
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        private readonly ICurrentUserService _currentUser;
        private readonly TimeProvider _timeProvider;

        public ItemRepository(IDbConnectionFactory connectionFactory, ICurrentUserService currentUser, TimeProvider? timeProvider = null) : base(connectionFactory)
        {
            _currentUser = currentUser;
            _timeProvider = timeProvider ?? TimeProvider.System;
        }

        public override async Task<IReadOnlyList<Item>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Item>("[Inventory].[usp_Item_ListAll]", new { UserID = _currentUser.UserID }, cancellationToken);

        public async Task<LENA.Application.Models.PagedResult<Item>> ListPagedAsync(int pageNumber, int pageSize, string? search = null, string? brand = null, bool inStock = false, bool isFavorite = false, CancellationToken ct = default)
            => await QueryPagedListAsync<Item>("[Inventory].[usp_Item_ListAllPaged]", pageNumber, pageSize, new { UserID = _currentUser.UserID, Search = search, Brand = brand, InStock = inStock, IsFavorite = isFavorite }, ct);

        public async Task<IReadOnlyList<Item>> SearchAsync(string search, string? brand, int limit, CancellationToken ct = default)
            => await QueryListAsync<Item>("[Inventory].[usp_Item_Search]", new { UserID = _currentUser.UserID, Search = search, Brand = brand, Limit = limit }, ct);

        public async Task<IReadOnlyList<string>> GetBrandsAsync(string? search = null, CancellationToken ct = default)
            => await QueryListAsync<string>("[Inventory].[usp_Item_GetBrands]", new { UserID = _currentUser.UserID, Search = search }, ct);

        public override async Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Item>("[Inventory].[usp_Item_GetById]", new { Id = id, UserID = _currentUser.UserID }, cancellationToken);

        public async Task<Item?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Item>("[Inventory].[usp_Item_GetByName]", new { Name = name, UserID = _currentUser.UserID }, cancellationToken);

        public override async Task<Item> CreateAsync(Item entity, CancellationToken cancellationToken = default)
        {
            entity.ItemID = await QuerySingleAsync<int>("[Inventory].[usp_Item_Create]", ToParameters(entity, false), cancellationToken);
            return entity;
        }

        public override async Task<Item> UpdateAsync(Item entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_Item_Update]", ToParameters(entity, true), nameof(Item), entity.ItemID, cancellationToken);
            return entity;
        }

        private DynamicParameters ToParameters(Item entity, bool forUpdate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", entity.Name);
            parameters.Add("Brand", entity.Brand);
            parameters.Add("UPC12", entity.UPC12);
            parameters.Add("UPC14", entity.UPC14);
            parameters.Add("CategoryID", entity.CategoryID);
            parameters.Add("Unit", entity.Unit);
            parameters.Add("UserID", _currentUser.UserID);
            parameters.Add("CurrentQuantity", entity.CurrentQuantity);
            parameters.Add("MinQuantity", entity.MinQuantity);
            parameters.Add("PurchaseDate", entity.PurchaseDate);
            parameters.Add("ExpiryDate", entity.ExpiryDate);
            parameters.Add("Notes", entity.Notes);
            parameters.Add("IsFavorite", entity.IsFavorite);

            if (forUpdate)
            {
                parameters.Add("ItemID", entity.ItemID);
                parameters.Add("LastUpdatedBy", entity.LastUpdatedBy);
                parameters.Add("LastUpdatedDate", entity.LastUpdatedDate);
            }
            else
            {
                parameters.Add("CreatedBy", entity.CreatedBy);
                parameters.Add("CreateDate", entity.CreateDate);
            }

            return parameters;
        }

        public override async Task<Item> DeleteAsync(Item entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_Item_Delete]", new { entity.ItemID, UserID = _currentUser.UserID }, nameof(Item), entity.ItemID, cancellationToken);
            return entity;
        }

        public async Task ChangeItemCategoryAsync(int itemId, int newCategoryId, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_ChangeItemCategory]", new { ItemID = itemId, CategoryID = newCategoryId }, cancellationToken);

        public async Task AddOrUpdateUPC12Async(int itemId, string upc12, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_AddOrUpdateUPC12]", new { ItemID = itemId, UPC12 = upc12 }, cancellationToken);

        public async Task AddOrUpdateUPC14Async(int itemId, string upc14, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_AddOrUpdateUPC14]", new { ItemID = itemId, UPC14 = upc14 }, cancellationToken);

        public async Task AdjustQuantityAsync(int itemId, decimal quantity, DateTime? purchaseDate = null, string? lastUpdatedBy = null, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_AdjustQuantity]", new { ItemID = itemId, UserID = _currentUser.UserID, Quantity = quantity, PurchaseDate = purchaseDate, LastUpdatedBy = lastUpdatedBy }, cancellationToken);

        public async Task SetFavoriteAsync(int itemId, bool isFavorite, CancellationToken cancellationToken = default)
        {
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            await ExecuteCommandAsync("[Inventory].[usp_Item_SetFavorite]", new
            {
                ItemID = itemId,
                UserID = _currentUser.UserID,
                IsFavorite = isFavorite,
                CreatedBy = _currentUser.UserName,
                CreateDate = now,
                LastUpdatedBy = _currentUser.UserName,
                LastUpdatedDate = now
            }, cancellationToken);
        }
    }
}