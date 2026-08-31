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

        public override async Task<IReadOnlyList<NutrientType>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT nutrient_id AS NutrientId,
                       nutrient_name AS NutrientName,
                       unit_of_measure AS UnitOfMeasure
                FROM [Inventory].[nutrient_types]
                ORDER BY nutrient_name";
            return (IReadOnlyList<NutrientType>)await connection.QueryAsync<NutrientType>(sql);
        }

        public override async Task<NutrientType?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT nutrient_id AS NutrientId,
                       nutrient_name AS NutrientName,
                       unit_of_measure AS UnitOfMeasure
                FROM [Inventory].[nutrient_types]
                WHERE nutrient_id = @Id";
            return await connection.QueryFirstOrDefaultAsync<NutrientType>(sql, new { Id = id });
        }

        public override async Task<NutrientType?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                SELECT nutrient_id AS NutrientId,
                       nutrient_name AS NutrientName,
                       unit_of_measure AS UnitOfMeasure
                FROM [Inventory].[nutrient_types]
                WHERE nutrient_name = @Name";
            return await connection.QueryFirstOrDefaultAsync<NutrientType>(sql, new { Name = name });
        }

        public override async Task<NutrientType> CreateAsync(NutrientType entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                INSERT INTO [Inventory].[nutrient_types] (nutrient_name, unit_of_measure)
                VALUES (@NutrientName, @UnitOfMeasure);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.NutrientId = id;
            return entity;
        }

        public override async Task<NutrientType> UpdateAsync(NutrientType entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                UPDATE [Inventory].[nutrient_types]
                SET nutrient_name = @NutrientName,
                    unit_of_measure = @UnitOfMeasure
                WHERE nutrient_id = @NutrientId";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<NutrientType> DeleteAsync(NutrientType entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Inventory].[nutrient_types] WHERE nutrient_id = @NutrientId";
            await connection.ExecuteAsync(sql, new { entity.NutrientId });
            return entity;
        }
    }
}
