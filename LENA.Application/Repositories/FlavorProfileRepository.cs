using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class FlavorProfileRepository : BaseRepository<FlavorProfile>, IFlavorProfileRepository
    {
        public FlavorProfileRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<FlavorProfile> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Inventory].[FlavorProfile] WHERE FlavorName = @Name";
            return (await connection.QueryFirstOrDefaultAsync<FlavorProfile>(sql, new { Name = name }))!;
        }

        public async Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Inventory].[FlavorProfile] WHERE IsActive = 1 ORDER BY FlavorName";
            return (IReadOnlyList<FlavorProfile>)await connection.QueryAsync<FlavorProfile>(sql);
        }
    }
}
