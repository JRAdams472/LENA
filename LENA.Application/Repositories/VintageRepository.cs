using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using System.Data;

namespace LENA.Application.Repositories
{
    public class VintageRepository : BaseRepository<Vintage>, IVintageRepository
    {
        public VintageRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<Vintage> CreateAsync(Vintage entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Vintage_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.VintageID = id;
            return entity;
        }

        public override async Task<Vintage?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Vintage>(
                "[Wine].[usp_Vintage_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<IReadOnlyList<Vintage>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Vintage>)await connection.QueryAsync<Vintage>(
                "[Wine].[usp_Vintage_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Vintage> UpdateAsync(Vintage entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Vintage_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Vintage> DeleteAsync(Vintage entitey, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Vintage_Delete]",
                new { entitey.VintageID },
                commandType: CommandType.StoredProcedure);
            return entitey;
        }

        public override async Task<Vintage?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult<Vintage?>(null);
        }

        public async Task<Vintage?> GetByYearAsync(int year, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Vintage>(
                "[Wine].[usp_Vintage_GetByYear]",
                new { Year = year },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<Vintage>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Vintage>)await connection.QueryAsync<Vintage>(
                "[Wine].[usp_Vintage_GetAllActive]",
                commandType: CommandType.StoredProcedure);
        }

    }
}
