using System.Data;
using System.Data.Common;
using Dapper;
using LENA.Application.Contracts.Persistence;

namespace LENA.Application.Repositories
{
    public abstract class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly IDbConnectionFactory _connectionFactory;

        protected BaseRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public abstract Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public abstract Task<T?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        public abstract Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        public abstract Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        protected async Task<IReadOnlyList<TResult>> QueryListAsync<TResult>(string sql, object? param = null, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition(sql, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return new List<TResult>(await connection.QueryAsync<TResult>(command));
        }

        protected async Task<TResult?> QueryFirstAsync<TResult>(string sql, object? param = null, CancellationToken cancellationToken = default) where TResult : class
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition(sql, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await connection.QueryFirstOrDefaultAsync<TResult>(command);
        }

        protected async Task<TResult> QuerySingleAsync<TResult>(string sql, object? param = null, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition(sql, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await connection.QuerySingleAsync<TResult>(command);
        }

        protected async Task<int> ExecuteCommandAsync(string sql, object? param = null, CancellationToken cancellationToken = default)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition(sql, param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
            return await connection.ExecuteAsync(command);
        }
    }
}
