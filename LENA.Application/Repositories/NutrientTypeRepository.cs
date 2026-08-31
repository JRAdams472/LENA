using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Repositories
{
    public class NutrientTypeRepository : BaseWineRepository<NutrientType>, INutrientTypeRepository
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