using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Repositories
{
    public class RegionRepository : BaseRepository<Region>, IRegionRepository
    {
        public RegionRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IReadOnlyList<Region>> GetAllByCountryIdAsync(int countryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Region] WHERE CountryID = @CountryId ORDER BY RegionName";
            return (IReadOnlyList<Region>)await connection.QueryAsync<Region>(sql, new { CountryId = countryId });
        }

        public async Task<Region?> GetByNameAndCountryIdAsync(string name, int countryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Region] WHERE RegionName = @Name AND CountryID = @CountryId";
            return await connection.QueryFirstOrDefaultAsync<Region>(sql, new { Name = name, CountryId = countryId });
        }

        public override async Task<Region> CreateAsync(Region entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"INSERT INTO [Wine].[Region] 
                       (RegionName, Description, IsActive, CountryID, CreatedBy, CreateDate) 
                       VALUES (@RegionName, @Description, @IsActive, @CountryID, @CreatedBy, @CreateDate);
                       SELECT CAST(SCOPE_IDENTITY() as int);";
            
            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.RegionID = id;
            return entity;
        }

        public override async Task<Region?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Region] WHERE RegionID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Region>(sql, new { Id = id });
        }

        public override async Task<IReadOnlyList<Region>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Region] ORDER BY RegionName";
            return (IReadOnlyList<Region>)await connection.QueryAsync<Region>(sql);
        }

        public override async Task<Region> UpdateAsync(Region entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"UPDATE [Wine].[Region] 
                       SET RegionName = @RegionName, Description = @Description, IsActive = @IsActive,
                           CountryID = @CountryID, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
                       WHERE RegionID = @RegionID";
            
            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<Region> DeleteAsync(Region entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Wine].[Region] WHERE RegionID = @RegionID";
            await connection.ExecuteAsync(sql, new { RegionID = entity.RegionID });
            return entity;
        }

        public override async Task<Region?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Wine].[Region] WHERE RegionName = @Name";
            return await connection.QueryFirstOrDefaultAsync<Region>(sql, new { Name = name });
        }

        public async Task<IReadOnlyList<Region>> GetAllByRegionIdAsync(int regionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Region>> GetAllByTypeIdAsync(int typeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Region>> GetAllByVintageYearAsync(int vintageYear)
        {
            throw new NotImplementedException();
        }
    }
}