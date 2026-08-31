using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class BottleRepository : BaseRepository<Bottle>, IBottleRepository
    {
        public BottleRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByCountryIdAsync(int countryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] WHERE CountryID = @CountryId ORDER BY BottleNumber";
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(sql, new { CountryId = countryId });
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByRegionIdAsync(int regionId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] WHERE RegionID = @RegionId ORDER BY BottleNumber";
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(sql, new { RegionId = regionId });
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByTypeIdAsync(int typeId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] WHERE TypeID = @TypeId ORDER BY BottleNumber";
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(sql, new { TypeId = typeId });
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByVintageYearAsync(int vintageYear)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] WHERE VintageYear = @VintageYear ORDER BY BottleNumber";
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(sql, new { VintageYear = vintageYear });
        }

        public async Task<IReadOnlyList<Bottle>> GetFavoritesAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] WHERE IsFavorite = 1 ORDER BY BottleNumber";
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(sql);
        }

        public async Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"SELECT * FROM [Wine].[Bottle] 
                       WHERE (BottleNumber IS NOT NULL AND CAST(BottleNumber AS NVARCHAR(10)) LIKE @SearchTerm)
                          OR (Vineyard LIKE @SearchTerm)
                          OR (GrapeVariety LIKE @SearchTerm)
                          OR (Notes LIKE @SearchTerm)
                       ORDER BY BottleNumber";
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(sql, new { SearchTerm = $"%{searchTerm}%" });
        }

        public async Task<int> GetTotalBottleCountAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT COUNT(*) FROM [Wine].[Bottle]";
            return await connection.QuerySingleAsync<int>(sql);
        }

        public override async Task<Bottle> CreateAsync(Bottle entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"INSERT INTO [Wine].[Bottle] 
                       (BottleNumber, TypeID, CountryID, RegionID, VintageYear, Vineyard, GrapeVariety, ABV, BottleSize, Quantity,
                        PurchaseDate, PurchasePrice, StorageTemp, Location, Notes, IsFavorite, CreatedBy, CreateDate) 
                       VALUES (@BottleNumber, @TypeID, @CountryID, @RegionID, @VintageYear, @Vineyard, @GrapeVariety, @ABV, @BottleSize,
                               @Quantity, @PurchaseDate, @PurchasePrice, @StorageTemp, @Location, @Notes, @IsFavorite, @CreatedBy, @CreateDate);
                       SELECT CAST(SCOPE_IDENTITY() as int);";
            
            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.BottleID = id;
            return entity;
        }

        public override async Task<Bottle?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] WHERE BottleID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Bottle>(sql, new { Id = id });
        }

        public override async Task<IReadOnlyList<Bottle>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] ORDER BY BottleNumber";
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(sql);
        }

        public override async Task<Bottle> UpdateAsync(Bottle entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"UPDATE [Wine].[Bottle] 
                       SET BottleNumber = @BottleNumber, TypeID = @TypeID, CountryID = @CountryID, RegionID = @RegionID,
                           VintageYear = @VintageYear, Vineyard = @Vineyard, GrapeVariety = @GrapeVariety, ABV = @ABV,
                           BottleSize = @BottleSize, Quantity = @Quantity, PurchaseDate = @PurchaseDate,
                           PurchasePrice = @PurchasePrice, StorageTemp = @StorageTemp, Location = @Location,
                           Notes = @Notes, IsFavorite = @IsFavorite, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
                       WHERE BottleID = @BottleID";
            
            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<Bottle> DeleteAsync(Bottle entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Wine].[Bottle] WHERE BottleID = @BottleID";
            await connection.ExecuteAsync(sql, new { BottleID = entity.BottleID });
            return entity;
        }

        public override async Task<Bottle?> GetByNameAsync(string name)
        {
            // Bottle entities don't have a name field in the database schema, so we'll search by vineyard
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Bottle] WHERE Vineyard = @Name";
            return await connection.QueryFirstOrDefaultAsync<Bottle>(sql, new { Name = name });
        }
    }
}