using System.Data;
using System.Data.Common;

using Dapper;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Application.Models;

namespace LENA.Infrastructure.Persistence
{
    public abstract class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected IDbConnectionFactory ConnectionFactory { get; }

        protected BaseRepository(IDbConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public abstract Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public abstract Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        protected async Task<IReadOnlyList<TResult>> QueryListAsync<TResult>(string procedureName, object? param = null, CancellationToken cancellationToken = default)
        {
            await using var connection = await ConnectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(procedureName, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return new List<TResult>(await connection.QueryAsync<TResult>(command));
        }

        protected async Task<PagedResult<TResult>> QueryPagedListAsync<TResult>(string procedureName, int pageNumber, int pageSize, object? param = null, CancellationToken ct = default)
        {
            var parameters = new DynamicParameters();
            if (param != null)
            {
                parameters.AddDynamicParams(param);
            }
            parameters.Add("PageNumber", pageNumber);
            parameters.Add("PageSize", pageSize);

            await using var connection = await ConnectionFactory.CreateConnectionAsync(ct);
            var command = new CommandDefinition(procedureName, parameters, commandType: CommandType.StoredProcedure, cancellationToken: ct);
            using var reader = await connection.QueryMultipleAsync(command);
            var items = (await reader.ReadAsync<TResult>()).ToList();
            var total = (await reader.ReadAsync<int>()).Single();
            return new PagedResult<TResult>
            {
                Items = items,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

        protected async Task<TResult?> QueryFirstAsync<TResult>(string procedureName, object? param = null, CancellationToken cancellationToken = default) where TResult : class
        {
            await using var connection = await ConnectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(procedureName, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await connection.QueryFirstOrDefaultAsync<TResult>(command);
        }

        protected async Task<TResult> QuerySingleAsync<TResult>(string procedureName, object? param = null, CancellationToken cancellationToken = default)
        {
            await using var connection = await ConnectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(procedureName, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await connection.QuerySingleAsync<TResult>(command);
        }

        protected async Task<int> ExecuteCommandAsync(string procedureName, object? param = null, CancellationToken cancellationToken = default)
        {
            await using var connection = await ConnectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(procedureName, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await connection.ExecuteAsync(command);
        }

        /// <summary>
        /// Runs a write procedure whose final statement is SELECT @@ROWCOUNT and throws when it matched no row.
        /// </summary>
        protected async Task ExecuteRequiringMatchAsync(string procedureName, object param, string entityName, object key, CancellationToken cancellationToken = default)
        {
            var affected = await QuerySingleAsync<int>(procedureName, param, cancellationToken);
            if (affected == 0)
            {
                throw new NotFoundException(entityName, key);
            }
        }
    }
}