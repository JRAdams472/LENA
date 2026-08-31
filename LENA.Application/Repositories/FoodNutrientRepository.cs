using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using System.Data;

namespace LENA.Application.Repositories
{
    public class FoodNutrientRepository : BaseRepository<FoodNutrient>, IFoodNutrientRepository
    {
        public FoodNutrientRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<FoodNutrient>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<FoodNutrient>)await connection.QueryAsync<FoodNutrient>(
                "[Inventory].[usp_FoodNutrient_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<FoodNutrient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<FoodNutrient>(
                "[Inventory].[usp_FoodNutrient_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<FoodNutrient?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult<FoodNutrient?>(null);
        }

        public override async Task<FoodNutrient> CreateAsync(FoodNutrient entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FoodNutrient_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<FoodNutrient> UpdateAsync(FoodNutrient entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FoodNutrient_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<FoodNutrient> DeleteAsync(FoodNutrient entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FoodNutrient_Delete]",
                new { entity.FoodId, entity.NutrientId },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public async Task<IEnumerable<FoodNutrient>> GetByFoodIdAsync(int foodId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<FoodNutrient>(
                "[Inventory].[usp_FoodNutrient_GetByFoodId]",
                new { FoodId = foodId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FoodNutrient>> GetByNutrientIdAsync(int nutrientId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<FoodNutrient>(
                "[Inventory].[usp_FoodNutrient_GetByNutrientId]",
                new { NutrientId = nutrientId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<FoodNutrient?> GetByFoodAndNutrientIdAsync(int foodId, int nutrientId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<FoodNutrient>(
                "[Inventory].[usp_FoodNutrient_GetByFoodAndNutrientId]",
                new { FoodId = foodId, NutrientId = nutrientId },
                commandType: CommandType.StoredProcedure);
        }
    }
}
