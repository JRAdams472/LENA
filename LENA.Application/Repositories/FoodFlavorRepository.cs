using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using System.Data;

namespace LENA.Application.Repositories
{
    public class FoodFlavorRepository : BaseRepository<FoodFlavor>, IFoodFlavorRepository
    {
        public FoodFlavorRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<FoodFlavor>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<FoodFlavor>)await connection.QueryAsync<FoodFlavor>(
                "[Inventory].[usp_FoodFlavor_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<FoodFlavor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<FoodFlavor>(
                "[Inventory].[usp_FoodFlavor_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<FoodFlavor?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult<FoodFlavor?>(null);
        }

        public override async Task<FoodFlavor> CreateAsync(FoodFlavor entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FoodFlavor_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<FoodFlavor> UpdateAsync(FoodFlavor entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FoodFlavor_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<FoodFlavor> DeleteAsync(FoodFlavor entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FoodFlavor_Delete]",
                new { entity.FoodId, entity.FlavorId },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public async Task<IEnumerable<FoodFlavor>> GetByFoodIdAsync(int foodId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<FoodFlavor>(
                "[Inventory].[usp_FoodFlavor_GetByFoodId]",
                new { FoodId = foodId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<FoodFlavor>> GetByFlavorIdAsync(int flavorId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<FoodFlavor>(
                "[Inventory].[usp_FoodFlavor_GetByFlavorId]",
                new { FlavorId = flavorId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<FoodFlavor?> GetByFoodAndFlavorIdAsync(int foodId, int flavorId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<FoodFlavor>(
                "[Inventory].[usp_FoodFlavor_GetByFoodAndFlavorId]",
                new { FoodId = foodId, FlavorId = flavorId },
                commandType: CommandType.StoredProcedure);
        }
    }
}
