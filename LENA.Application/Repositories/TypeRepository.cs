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
