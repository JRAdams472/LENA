using System.Data;
using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Identity;

namespace LENA.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByExternalSubjectAsync(string externalSubject, string provider, CancellationToken cancellationToken = default)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(
                "[Identity].[usp_User_GetByExternalSubject]",
                new { Provider = provider, ExternalSubject = externalSubject },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            return await connection.QueryFirstOrDefaultAsync<User>(command);
        }

        public async Task<User> UpsertAsync(User user, CancellationToken cancellationToken = default)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(
                "[Identity].[usp_User_Upsert]",
                new
                {
                    user.Provider,
                    user.ExternalSubject,
                    user.Email,
                    user.DisplayName,
                    user.CreatedBy,
                    user.CreateDate,
                    user.LastUpdatedBy,
                    user.LastUpdatedDate,
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            return await connection.QuerySingleAsync<User>(command);
        }
    }
}
