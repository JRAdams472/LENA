using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class FoodFlavorRepository : BaseRepository<FoodFlavor>, IFoodFlavorRepository
    {
        public FoodFlavorRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<FoodFlavor>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       flavor_id AS FlavorId,
                       intensity_score AS IntensityScore
                FROM [Inventory].[food_flavors]";
            return (IReadOnlyList<FoodFlavor>)await connection.QueryAsync<FoodFlavor>(sql);
        }

        public override async Task<FoodFlavor?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       flavor_id AS FlavorId,
                       intensity_score AS IntensityScore
                FROM [Inventory].[food_flavors]
                WHERE food_id = @Id";
            return await connection.QueryFirstOrDefaultAsync<FoodFlavor>(sql, new { Id = id });
        }

        public override async Task<FoodFlavor?> GetByNameAsync(string name)
        {
            return await Task.FromResult<FoodFlavor?>(null);
        }

        public override async Task<FoodFlavor> CreateAsync(FoodFlavor entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                INSERT INTO [Inventory].[food_flavors] (food_id, flavor_id, intensity_score)
                VALUES (@FoodId, @FlavorId, @IntensityScore)";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<FoodFlavor> UpdateAsync(FoodFlavor entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                UPDATE [Inventory].[food_flavors]
                SET intensity_score = @IntensityScore
                WHERE food_id = @FoodId AND flavor_id = @FlavorId";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<FoodFlavor> DeleteAsync(FoodFlavor entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Inventory].[food_flavors] WHERE food_id = @FoodId AND flavor_id = @FlavorId";
            await connection.ExecuteAsync(sql, new { entity.FoodId, entity.FlavorId });
            return entity;
        }

        public async Task<IEnumerable<FoodFlavor>> GetByFoodIdAsync(int foodId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       flavor_id AS FlavorId,
                       intensity_score AS IntensityScore
                FROM [Inventory].[food_flavors]
                WHERE food_id = @FoodId";
            return await connection.QueryAsync<FoodFlavor>(sql, new { FoodId = foodId });
        }

        public async Task<IEnumerable<FoodFlavor>> GetByFlavorIdAsync(int flavorId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       flavor_id AS FlavorId,
                       intensity_score AS IntensityScore
                FROM [Inventory].[food_flavors]
                WHERE flavor_id = @FlavorId";
            return await connection.QueryAsync<FoodFlavor>(sql, new { FlavorId = flavorId });
        }

        public async Task<FoodFlavor?> GetByFoodAndFlavorIdAsync(int foodId, int flavorId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       flavor_id AS FlavorId,
                       intensity_score AS IntensityScore
                FROM [Inventory].[food_flavors]
                WHERE food_id = @FoodId AND flavor_id = @FlavorId";
            return await connection.QueryFirstOrDefaultAsync<FoodFlavor>(sql, new { FoodId = foodId, FlavorId = flavorId });
        }
    }
}
