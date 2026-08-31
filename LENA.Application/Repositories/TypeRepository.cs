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

        public override async Task<TypeEntity> CreateAsync(TypeEntity entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Wine].[usp_Type_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.TypeID = id;
            return entity;
        }

        public override async Task<TypeEntity?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<TypeEntity>(
                "[Wine].[usp_Type_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<IReadOnlyList<TypeEntity>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<TypeEntity>)await connection.QueryAsync<TypeEntity>(
                "[Wine].[usp_Type_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<TypeEntity> UpdateAsync(TypeEntity entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Type_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<TypeEntity> DeleteAsync(TypeEntity entitey)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Wine].[usp_Type_Delete]",
                new { entitey.TypeID },
                commandType: CommandType.StoredProcedure);
            return entitey;
        }

        public override async Task<TypeEntity?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<TypeEntity>(
                "[Wine].[usp_Type_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

        public Task<IReadOnlyList<TypeEntity>> GetAllByCountryIdAsync(int countryId)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<TypeEntity>> GetAllByRegionIdAsync(int regionId)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<TypeEntity>> GetAllByTypeIdAsync(int typeId)
            => throw new NotImplementedException();

        public Task<IReadOnlyList<TypeEntity>> GetAllByVintageYearAsync(int vintageYear)
            => throw new NotImplementedException();
    }
}
