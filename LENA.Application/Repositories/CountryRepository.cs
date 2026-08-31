using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<Country?> GetByISOCodeAsync(string isoCode)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Country] WHERE ISOCode = @ISOCode";
            return await connection.QueryFirstOrDefaultAsync<Country>(sql, new { ISOCode = isoCode });
        }

        public async Task<IReadOnlyList<Country>> GetAllActiveAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Country] WHERE IsActive = 1 ORDER BY CountryName";
            return (IReadOnlyList<Country>)await connection.QueryAsync<Country>(sql);
        }

        public override async Task<Country> CreateAsync(Country entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"INSERT INTO [Wine].[Country] 
                       (CountryName, ISOCode, Description, IsActive, CreatedBy, CreateDate) 
                       VALUES (@CountryName, @ISOCode, @Description, @IsActive, @CreatedBy, @CreateDate);
                       SELECT CAST(SCOPE_IDENTITY() as int);";
            
            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.CountryID = id;
            return entity;
        }

        public override async Task<Country?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Country] WHERE CountryID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Country>(sql, new { Id = id });
        }

        public override async Task<IReadOnlyList<Country>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Country] ORDER BY CountryName";
            return (IReadOnlyList<Country>)await connection.QueryAsync<Country>(sql);
        }

        public override async Task<Country> UpdateAsync(Country entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"UPDATE [Wine].[Country] 
                       SET CountryName = @CountryName, ISOCode = @ISOCode, Description = @Description, 
                           IsActive = @IsActive, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
                       WHERE CountryID = @CountryID";
            
            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<Country> DeleteAsync(Country entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Wine].[Country] WHERE CountryID = @CountryID";
            await connection.ExecuteAsync(sql, new { CountryID = entity.CountryID });
            return entity;
        }

        public override async Task<Country?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Country] WHERE CountryName = @Name";
            return await connection.QueryFirstOrDefaultAsync<Country>(sql, new { Name = name });
        }

        public async Task<IReadOnlyList<Country>> GetAllByCountryIdAsync(int countryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Country>> GetAllByRegionIdAsync(int regionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Country>> GetAllByTypeIdAsync(int typeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Country>> GetAllByVintageYearAsync(int vintageYear)
        {
            throw new NotImplementedException();
        }
    }
}