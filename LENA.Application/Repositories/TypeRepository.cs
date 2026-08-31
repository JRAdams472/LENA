using Dapper;
using LENA.Application.Contracts.Persistence;
using System.Data;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Repositories
{
    public class TypeRepository : BaseRepository<TypeEntity>, ITypeRepository
    {
        public TypeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<TypeEntity> CreateAsync(TypeEntity entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Type_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.TypeID = id;
            return entity;
        }

        public override async Task<TypeEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<TypeEntity>(
                "[Wine].[usp_Type_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<IReadOnlyList<TypeEntity>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<TypeEntity>)await connection.QueryAsync<TypeEntity>(
                "[Wine].[usp_Type_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<TypeEntity> UpdateAsync(TypeEntity entity, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Type_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<TypeEntity> DeleteAsync(TypeEntity entitey, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Type_Delete]",
                new { entitey.TypeID },
                commandType: CommandType.StoredProcedure);
            return entitey;
        }

        public override async Task<TypeEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<TypeEntity>(
                "[Wine].[usp_Type_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

    }
}
