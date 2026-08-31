using Dapper;
using LENA.Application.Contracts.Persistence;
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
            var sql = @"INSERT INTO [Wine].[Type] (TypeName, Description, IsActive, CreatedBy, CreateDate)
                        VALUES (@TypeName, @Description, @IsActive, @CreatedBy, @CreateDate);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.TypeID = id;
            return entity;
        }

        public override async Task<TypeEntity?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Type] WHERE TypeID = @Id";
            return await connection.QueryFirstOrDefaultAsync<TypeEntity>(sql, new { Id = id });
        }

        public override async Task<IReadOnlyList<TypeEntity>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Type] ORDER BY TypeName";
            return (IReadOnlyList<TypeEntity>)await connection.QueryAsync<TypeEntity>(sql);
        }

        public override async Task<TypeEntity> UpdateAsync(TypeEntity entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"UPDATE [Wine].[Type]
                        SET TypeName = @TypeName, Description = @Description, IsActive = @IsActive,
                            LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
                        WHERE TypeID = @TypeID";
            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<TypeEntity> DeleteAsync(TypeEntity entitey)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Wine].[Type] WHERE TypeID = @TypeID";
            await connection.ExecuteAsync(sql, new { entitey.TypeID });
            return entitey;
        }

        public override async Task<TypeEntity?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Type] WHERE TypeName = @Name";
            return await connection.QueryFirstOrDefaultAsync<TypeEntity>(sql, new { Name = name });
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
