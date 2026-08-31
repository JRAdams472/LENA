using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class VintageRepository : BaseRepository<Vintage>, IVintageRepository
    {
        public VintageRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<Vintage> CreateAsync(Vintage entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"INSERT INTO [Wine].[Vintage] (Year, Description, IsActive, CreatedBy, CreateDate)
                        VALUES (@Year, @Description, @IsActive, @CreatedBy, @CreateDate);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.VintageID = id;
            return entity;
        }

        public override async Task<Vintage?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Vintage] WHERE VintageID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Vintage>(sql, new { Id = id });
        }

        public override async Task<IReadOnlyList<Vintage>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Vintage] ORDER BY Year";
            return (IReadOnlyList<Vintage>)await connection.QueryAsync<Vintage>(sql);
        }

        public override async Task<Vintage> UpdateAsync(Vintage entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"UPDATE [Wine].[Vintage]
                        SET Year = @Year, Description = @Description, IsActive = @IsActive,
                            LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
                        WHERE VintageID = @VintageID";
            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<Vintage> DeleteAsync(Vintage entitey)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Wine].[Vintage] WHERE VintageID = @VintageID";
            await connection.ExecuteAsync(sql, new { entitey.VintageID });
            return entitey;
        }

        public override async Task<Vintage?> GetByNameAsync(string name)
        {
            return await Task.FromResult<Vintage?>(null);
        }

        public async Task<Vintage?> GetByYearAsync(int year)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Vintage] WHERE Year = @Year";
            return await connection.QueryFirstOrDefaultAsync<Vintage>(sql, new { Year = year });
        }

        public async Task<IReadOnlyList<Vintage>> GetAllActiveAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Vintage] WHERE IsActive = 1 ORDER BY Year";
            return (IReadOnlyList<Vintage>)await connection.QueryAsync<Vintage>(sql);
        }

        public Task<IReadOnlyList<Vintage>> GetAllByCountryIdAsync(int countryId)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<Vintage>> GetAllByRegionIdAsync(int regionId)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<Vintage>> GetAllByTypeIdAsync(int typeId)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<Vintage>> GetAllByVintageYearAsync(int vintageYear)
            => throw new NotImplementedException();
    }
}
