using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class NutrientTypeRepository : BaseRepository<NutrientType>, INutrientTypeRepository
    {
        public NutrientTypeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<NutrientType?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Inventory].[nutrient_types] WHERE nutrient_name = @Name";
            return await connection.QueryFirstOrDefaultAsync<NutrientType>(sql, new { Name = name });
        }
    }
}