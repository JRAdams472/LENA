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

        public override async Task<IReadOnlyList<FlavorProfile>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT flavor_id AS FlavorId,
                       flavor_name AS FlavorName,
                       is_active AS IsActive
                FROM [Inventory].[flavor_profiles]
                ORDER BY flavor_name";
            return (IReadOnlyList<FlavorProfile>)await connection.QueryAsync<FlavorProfile>(sql);
        }

        public override async Task<FlavorProfile?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT flavor_id AS FlavorId,
                       flavor_name AS FlavorName,
                       is_active AS IsActive
                FROM [Inventory].[flavor_profiles]
                WHERE flavor_id = @Id";
            return await connection.QueryFirstOrDefaultAsync<FlavorProfile>(sql, new { Id = id });
        }

        public override async Task<FlavorProfile?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT flavor_id AS FlavorId,
                       flavor_name AS FlavorName,
                       is_active AS IsActive
                FROM [Inventory].[flavor_profiles]
                WHERE flavor_name = @Name";
            return await connection.QueryFirstOrDefaultAsync<FlavorProfile>(sql, new { Name = name });
        }

        public override async Task<FlavorProfile> CreateAsync(FlavorProfile entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                INSERT INTO [Inventory].[flavor_profiles] (flavor_name, is_active)
                VALUES (@FlavorName, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.FlavorId = id;
            return entity;
        }

        public override async Task<FlavorProfile> UpdateAsync(FlavorProfile entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                UPDATE [Inventory].[flavor_profiles]
                SET flavor_name = @FlavorName,
                    is_active = @IsActive
                WHERE flavor_id = @FlavorId";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<FlavorProfile> DeleteAsync(FlavorProfile entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Inventory].[flavor_profiles] WHERE flavor_id = @FlavorId";
            await connection.ExecuteAsync(sql, new { entity.FlavorId });
            return entity;
        }

        public async Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT flavor_id AS FlavorId,
                       flavor_name AS FlavorName,
                       is_active AS IsActive
                FROM [Inventory].[flavor_profiles]
                WHERE is_active = 1
                ORDER BY flavor_name";
            return (IReadOnlyList<FlavorProfile>)await connection.QueryAsync<FlavorProfile>(sql);
        }
    }
}
