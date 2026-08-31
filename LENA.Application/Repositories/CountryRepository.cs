using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using System.Data;

namespace LENA.Application.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<Country?> GetByISOCodeAsync(string isoCode, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Country>(
                "[Wine].[usp_Country_GetByISOCode]",
                new { ISOCode = isoCode },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Country>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Country>)await connection.QueryAsync<Country>(
                "[Wine].[usp_Country_GetAllActive]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Country> CreateAsync(Country entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Country_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.CountryID = id;
            return entity;
        }

        public override async Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Country>(
                "[Wine].[usp_Country_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<IReadOnlyList<Country>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Country>)await connection.QueryAsync<Country>(
                "[Wine].[usp_Country_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Country> UpdateAsync(Country entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Country_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Country> DeleteAsync(Country entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Country_Delete]",
                new { CountryID = entity.CountryID },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Country>(
                "[Wine].[usp_Country_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

    }
}