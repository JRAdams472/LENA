using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using System.Data;

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
            return (IReadOnlyList<FlavorProfile>)await connection.QueryAsync<FlavorProfile>(
                "[Inventory].[usp_FlavorProfile_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<FlavorProfile?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<FlavorProfile>(
                "[Inventory].[usp_FlavorProfile_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<FlavorProfile?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<FlavorProfile>(
                "[Inventory].[usp_FlavorProfile_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<FlavorProfile> CreateAsync(FlavorProfile entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Inventory].[usp_FlavorProfile_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.FlavorId = id;
            return entity;
        }

        public override async Task<FlavorProfile> UpdateAsync(FlavorProfile entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FlavorProfile_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<FlavorProfile> DeleteAsync(FlavorProfile entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_FlavorProfile_Delete]",
                new { entity.FlavorId },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public async Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<FlavorProfile>)await connection.QueryAsync<FlavorProfile>(
                "[Inventory].[usp_FlavorProfile_GetAllActive]",
                commandType: CommandType.StoredProcedure);
        }
    }
}
