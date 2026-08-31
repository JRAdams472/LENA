using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Repositories
{
    public abstract class BaseWineRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly IDbConnectionFactory _connectionFactory;

        static BaseWineRepository()
        {
            var type = typeof(T);
            if (type.Namespace != "LENA.Domain.Entity.Wine")
            {
                throw new InvalidOperationException($"BaseWineRepository only supports entities in the LENA.Domain.Entity.Wine namespace. Type {type.FullName} is in {type.Namespace}.");
            }
        }

        protected BaseWineRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var type = typeof(T);
            var idProperty = type.GetProperty($"{type.Name}ID")
                ?? type.GetProperty("ID")
                ?? type.GetProperties().FirstOrDefault(p => p.Name.EndsWith("ID"));

            var schema = type.Namespace?.Split('.').Last() ?? "dbo";
            var tableName = type.Name;

            var properties = type.GetProperties()
                .Where(p => p != idProperty)
                .Where(p => IsSimpleType(p.PropertyType))
                .Where(p => p.CanRead && p.GetSetMethod() != null)
                .ToList();

            if (idProperty == null)
                throw new InvalidOperationException($"No ID property found for {type.Name}");

            var columns = string.Join(", ", properties.Select(p => $"[{p.Name}]"));
            var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $@"
                INSERT INTO [{schema}].[{tableName}] ({columns})
                VALUES ({parameters});
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = await connection.QuerySingleAsync<int>(sql, entity);
            idProperty.SetValue(entity, id);

            return entity;
        }

        private static bool IsSimpleType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            return underlyingType.IsPrimitive
                || underlyingType == typeof(string)
                || underlyingType == typeof(decimal)
                || underlyingType == typeof(DateTime)
                || underlyingType == typeof(Guid);
        }

        public virtual async Task<T?> GetByIdAsync(int Id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var type = typeof(T);
            var idProperty = type.GetProperty($"{type.Name}ID")
                ?? type.GetProperty("ID")
                ?? type.GetProperties().FirstOrDefault(p => p.Name.EndsWith("ID"));

            if (idProperty == null)
                throw new InvalidOperationException($"No ID property found for {type.Name}");

            var schema = type.Namespace?.Split('.').Last() ?? "dbo";
            var tableName = type.Name;

            var sql = $"SELECT * FROM [{schema}].[{tableName}] WHERE [{idProperty.Name}] = @Id";

            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id });
        }

        public virtual async Task<T?> GetByNameAsync(string Name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var type = typeof(T);
            var nameProperty = type.GetProperty("Name")
                ?? type.GetProperty($"{type.Name}Name")
                ?? type.GetProperties().FirstOrDefault(p => p.PropertyType == typeof(string) && p.Name.EndsWith("Name"));

            if (nameProperty == null)
                throw new InvalidOperationException($"No name property found for {type.Name}");

            var schema = type.Namespace?.Split('.').Last() ?? "dbo";
            var tableName = type.Name;

            var sql = $"SELECT * FROM [{schema}].[{tableName}] WHERE [{nameProperty.Name}] = @Name";

            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Name });
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var type = typeof(T);
            var idProperty = type.GetProperty($"{type.Name}ID")
                ?? type.GetProperty("ID")
                ?? type.GetProperties().FirstOrDefault(p => p.Name.EndsWith("ID"));

            if (idProperty == null)
                throw new InvalidOperationException($"No ID property found for {type.Name}");

            var schema = type.Namespace?.Split('.').Last() ?? "dbo";
            var tableName = type.Name;

            var properties = type.GetProperties()
                .Where(p => p != idProperty)
                .Where(p => IsSimpleType(p.PropertyType))
                .Where(p => p.CanRead && p.GetSetMethod() != null)
                .ToList();

            var setClauses = string.Join(", ", properties.Select(p => $"[{p.Name}] = @{p.Name}"));

            var sql = $@"
                UPDATE [{schema}].[{tableName}]
                SET {setClauses}
                WHERE [{idProperty.Name}] = @{idProperty.Name};";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public virtual async Task<T> DeleteAsync(T entitey)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var type = typeof(T);
            var idProperty = type.GetProperty($"{type.Name}ID")
                ?? type.GetProperty("ID")
                ?? type.GetProperties().FirstOrDefault(p => p.Name.EndsWith("ID"));

            if (idProperty == null)
                throw new InvalidOperationException($"No ID property found for {type.Name}");

            var schema = type.Namespace?.Split('.').Last() ?? "dbo";
            var tableName = type.Name;

            var id = idProperty.GetValue(entitey);
            var sql = $"DELETE FROM [{schema}].[{tableName}] WHERE [{idProperty.Name}] = @Id";

            await connection.ExecuteAsync(sql, new { Id = id });
            return entitey;
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            var type = typeof(T);
            var nameProperty = type.GetProperty("Name")
                ?? type.GetProperty($"{type.Name}Name")
                ?? type.GetProperties().FirstOrDefault(p => p.PropertyType == typeof(string) && p.Name.EndsWith("Name"));

            var schema = type.Namespace?.Split('.').Last() ?? "dbo";
            var tableName = type.Name;

            var orderBy = nameProperty != null ? $"ORDER BY [{nameProperty.Name}]" : "";
            var sql = $"SELECT * FROM [{schema}].[{tableName}] {orderBy}";

            var results = await connection.QueryAsync<T>(sql);
            return (IReadOnlyList<T>)results.ToList();
        }
    }
}