using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class FoodNutrientRepository : BaseRepository<FoodNutrient>, IFoodNutrientRepository
    {
        public FoodNutrientRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<FoodNutrient>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       nutrient_id AS NutrientId,
                       amount_per_serving AS AmountPerServing
                FROM [Inventory].[food_nutrients]";
            return (IReadOnlyList<FoodNutrient>)await connection.QueryAsync<FoodNutrient>(sql);
        }

        public override async Task<FoodNutrient?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       nutrient_id AS NutrientId,
                       amount_per_serving AS AmountPerServing
                FROM [Inventory].[food_nutrients]
                WHERE food_id = @Id";
            return await connection.QueryFirstOrDefaultAsync<FoodNutrient>(sql, new { Id = id });
        }

        public override async Task<FoodNutrient?> GetByNameAsync(string name)
        {
            return await Task.FromResult<FoodNutrient?>(null);
        }

        public override async Task<FoodNutrient> CreateAsync(FoodNutrient entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                INSERT INTO [Inventory].[food_nutrients] (food_id, nutrient_id, amount_per_serving)
                VALUES (@FoodId, @NutrientId, @AmountPerServing)";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<FoodNutrient> UpdateAsync(FoodNutrient entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                UPDATE [Inventory].[food_nutrients]
                SET amount_per_serving = @AmountPerServing
                WHERE food_id = @FoodId AND nutrient_id = @NutrientId";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<FoodNutrient> DeleteAsync(FoodNutrient entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Inventory].[food_nutrients] WHERE food_id = @FoodId AND nutrient_id = @NutrientId";
            await connection.ExecuteAsync(sql, new { entity.FoodId, entity.NutrientId });
            return entity;
        }

        public async Task<IEnumerable<FoodNutrient>> GetByFoodIdAsync(int foodId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       nutrient_id AS NutrientId,
                       amount_per_serving AS AmountPerServing
                FROM [Inventory].[food_nutrients]
                WHERE food_id = @FoodId";
            return await connection.QueryAsync<FoodNutrient>(sql, new { FoodId = foodId });
        }

        public async Task<IEnumerable<FoodNutrient>> GetByNutrientIdAsync(int nutrientId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       nutrient_id AS NutrientId,
                       amount_per_serving AS AmountPerServing
                FROM [Inventory].[food_nutrients]
                WHERE nutrient_id = @NutrientId";
            return await connection.QueryAsync<FoodNutrient>(sql, new { NutrientId = nutrientId });
        }

        public async Task<FoodNutrient?> GetByFoodAndNutrientIdAsync(int foodId, int nutrientId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT food_id AS FoodId,
                       nutrient_id AS NutrientId,
                       amount_per_serving AS AmountPerServing
                FROM [Inventory].[food_nutrients]
                WHERE food_id = @FoodId AND nutrient_id = @NutrientId";
            return await connection.QueryFirstOrDefaultAsync<FoodNutrient>(sql, new { FoodId = foodId, NutrientId = nutrientId });
        }
    }
}
