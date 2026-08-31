using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using System.Data;

namespace LENA.Application.Repositories
{
    public class NutrientTypeRepository : BaseRepository<NutrientType>, INutrientTypeRepository
    {
        public NutrientTypeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<NutrientType>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<NutrientType>)await connection.QueryAsync<NutrientType>(
                "[Inventory].[usp_NutrientType_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<NutrientType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<NutrientType>(
                "[Inventory].[usp_NutrientType_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<NutrientType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<NutrientType>(
                "[Inventory].[usp_NutrientType_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<NutrientType> CreateAsync(NutrientType entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Inventory].[usp_NutrientType_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.NutrientId = id;
            return entity;
        }

        public override async Task<NutrientType> UpdateAsync(NutrientType entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_NutrientType_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<NutrientType> DeleteAsync(NutrientType entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_NutrientType_Delete]",
                new { entity.NutrientId },
                commandType: CommandType.StoredProcedure);
            return entity;
        }
    }
}
