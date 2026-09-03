using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Identity;

namespace LENA.IntegrationTests.Infrastructure
{
    public class FakeUserRepository : IUserRepository
    {
        private const string TestExternalSubject = "test-sub";

        public Task<User?> GetByExternalSubjectAsync(
            string externalSubject,
            string provider,
            CancellationToken cancellationToken = default)
        {
            if (externalSubject == TestExternalSubject)
            {
                return Task.FromResult<User?>(new User
                {
                    UserID = 1,
                    Provider = provider,
                    ExternalSubject = externalSubject,
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    IsActive = true,
                    CreatedBy = "test",
                    CreateDate = DateTime.UtcNow,
                });
            }

            return Task.FromResult<User?>(null);
        }

        public Task<User> UpsertAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user.UserID == 0)
            {
                user.UserID = 1;
            }

            return Task.FromResult(user);
        }
    }
}