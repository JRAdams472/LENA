using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using System.Data;

namespace LENA.Application.Repositories
{
    public class BottleRepository : BaseRepository<Bottle>, IBottleRepository
    {
        public BottleRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByCountryIdAsync(int countryId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByCountryId]",
                new { CountryId = countryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByRegionIdAsync(int regionId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByRegionId]",
                new { RegionId = regionId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByTypeId]",
                new { TypeId = typeId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByVintageYearAsync(int vintageYear, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByVintageYear]",
                new { VintageYear = vintageYear },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetFavoritesAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetFavorites]",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition(
                "[Wine].[usp_Bottle_SearchBottles]",
                new { SearchTerm = searchTerm },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(command);
        }

        public async Task<int> GetTotalBottleCountAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Bottle_GetTotalBottleCount]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Bottle> CreateAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Bottle_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.BottleID = id;
            return entity;
        }

        public override async Task<Bottle?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Bottle>(
                "[Wine].[usp_Bottle_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<IReadOnlyList<Bottle>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Bottle> UpdateAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Bottle_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Bottle> DeleteAsync(Bottle entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Bottle_Delete]",
                new { BottleID = entity.BottleID },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Bottle?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Bottle>(
                "[Wine].[usp_Bottle_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }
    }
}