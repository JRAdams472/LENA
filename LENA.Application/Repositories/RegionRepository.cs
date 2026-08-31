using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using System.Data;

namespace LENA.Application.Repositories
{
    public class RegionRepository : BaseRepository<Region>, IRegionRepository
    {
        public RegionRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<Region>> GetAllByCountryIdAsync(int countryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Region>)await connection.QueryAsync<Region>(
                "[Wine].[usp_Region_GetAllByCountryId]",
                new { CountryId = countryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Region?> GetByNameAndCountryIdAsync(string name, int countryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Region>(
                "[Wine].[usp_Region_GetByNameAndCountryId]",
                new { Name = name, CountryId = countryId },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Region> CreateAsync(Region entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Region_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.RegionID = id;
            return entity;
        }

        public override async Task<Region?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Region>(
                "[Wine].[usp_Region_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<IReadOnlyList<Region>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Region>)await connection.QueryAsync<Region>(
                "[Wine].[usp_Region_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Region> UpdateAsync(Region entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Region_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Region> DeleteAsync(Region entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Region_Delete]",
                new { RegionID = entity.RegionID },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Region?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Region>(
                "[Wine].[usp_Region_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

    }
}