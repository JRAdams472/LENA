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

        public async Task<IReadOnlyList<Bottle>> GetAllByCountryIdAsync(int countryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByCountryId]",
                new { CountryId = countryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByRegionIdAsync(int regionId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByRegionId]",
                new { RegionId = regionId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByTypeIdAsync(int typeId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByTypeId]",
                new { TypeId = typeId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetAllByVintageYearAsync(int vintageYear)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetAllByVintageYear]",
                new { VintageYear = vintageYear },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> GetFavoritesAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_GetFavorites]",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Bottle>> SearchBottlesAsync(string searchTerm)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_SearchBottles]",
                new { SearchTerm = $"%{searchTerm}%" },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> GetTotalBottleCountAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Bottle_GetTotalBottleCount]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Bottle> CreateAsync(Bottle entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Bottle_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.BottleID = id;
            return entity;
        }

        public override async Task<Bottle?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Bottle>(
                "[Wine].[usp_Bottle_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<IReadOnlyList<Bottle>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Bottle>)await connection.QueryAsync<Bottle>(
                "[Wine].[usp_Bottle_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Bottle> UpdateAsync(Bottle entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Bottle_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Bottle> DeleteAsync(Bottle entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Bottle_Delete]",
                new { BottleID = entity.BottleID },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Bottle?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Bottle>(
                "[Wine].[usp_Bottle_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }
    }
}