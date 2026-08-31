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
    public class FoodNutrientRepository : BaseRepository<FoodNutrient>, IFoodNutrientRepository
    {
        public FoodNutrientRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<FoodNutrient>> GetByFoodIdAsync(int foodId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"SELECT fn.*, nt.nutrient_name, nt.unit_of_measure 
                        FROM [Inventory].[food_nutrients] fn
                        JOIN [Inventory].[nutrient_types] nt ON fn.nutrient_id = nt.nutrient_id
                        WHERE fn.food_id = @FoodId";
            return await connection.QueryAsync<FoodNutrient>(sql, new { FoodId = foodId });
        }

        public async Task<IEnumerable<FoodNutrient>> GetByNutrientIdAsync(int nutrientId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"SELECT fn.*, i.Name as ItemName
                        FROM [Inventory].[food_nutrients] fn
                        JOIN [Inventory].[Item] i ON fn.food_id = i.ItemID
                        WHERE fn.nutrient_id = @NutrientId";
            return await connection.QueryAsync<FoodNutrient>(sql, new { NutrientId = nutrientId });
        }

        public async Task<FoodNutrient?> GetByFoodAndNutrientIdAsync(int foodId, int nutrientId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"SELECT fn.*, nt.nutrient_name, nt.unit_of_measure
                        FROM [Inventory].[food_nutrients] fn
                        JOIN [Inventory].[nutrient_types] nt ON fn.nutrient_id = nt.nutrient_id
                        WHERE fn.food_id = @FoodId AND fn.nutrient_id = @NutrientId";
            return await connection.QueryFirstOrDefaultAsync<FoodNutrient>(sql, new { FoodId = foodId, NutrientId = nutrientId });
        }
    }
}