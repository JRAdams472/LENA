using LENA.Domain.Entity.Identity;

namespace LENA.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByExternalSubjectAsync(string externalSubject, string provider, CancellationToken cancellationToken = default);

        Task<User> UpsertAsync(User user, CancellationToken cancellationToken = default);
    }
}