using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Repositories
{
    public class FlavorProfileRepository : BaseWineRepository<FlavorProfile>, IFlavorProfileRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public FlavorProfileRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<FlavorProfile> GetByNameAsync(string name)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var sql = @"SELECT * FROM [Wine].[FlavorProfile] 
                        WHERE FlavorProfileName = @Name AND IsActive = 1";
            
            return await connection.QueryFirstOrDefaultAsync<FlavorProfile>(sql, new { Name = name });
        }

        public async Task<IReadOnlyList<FlavorProfile>> GetAllActiveAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var sql = @"SELECT * FROM [Wine].[FlavorProfile] 
                        WHERE IsActive = 1 
                        ORDER BY FlavorProfileName";
            
            return (IReadOnlyList<FlavorProfile>)await connection.QueryAsync<FlavorProfile>(sql);
        }
    }
}