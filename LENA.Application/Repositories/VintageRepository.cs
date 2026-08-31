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
