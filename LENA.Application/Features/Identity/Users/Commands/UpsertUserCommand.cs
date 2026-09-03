using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Identity;

using MediatR;

namespace LENA.Application.Features.Identity.Users.Commands
{
    public record UpsertUserCommand(string ExternalSubject, string Provider, string Email, string? DisplayName) : IRequest<User>;

    public class UpsertUserCommandHandler : IRequestHandler<UpsertUserCommand, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly TimeProvider _timeProvider;

        public UpsertUserCommandHandler(IUserRepository userRepository, ICurrentUserService currentUser, TimeProvider timeProvider)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _timeProvider = timeProvider;
        }

        public async Task<User> Handle(UpsertUserCommand request, CancellationToken cancellationToken)
        {
            var now = _timeProvider.GetUtcNow().UtcDateTime;

            var user = new User
            {
                ExternalSubject = request.ExternalSubject,
                Provider = request.Provider,
                Email = request.Email,
                DisplayName = request.DisplayName,
                IsActive = true,
                LastLoginDate = now,
                CreatedBy = _currentUser.UserName,
                CreateDate = now,
                LastUpdatedBy = _currentUser.UserName,
                LastUpdatedDate = now,
            };

            return await _userRepository.UpsertAsync(user, cancellationToken);
        }
    }
}