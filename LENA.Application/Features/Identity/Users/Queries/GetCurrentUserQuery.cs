using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Identity;
using MediatR;

namespace LENA.Application.Features.Identity.Users.Queries
{
    public record GetCurrentUserQuery : IRequest<User?>;

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, User?>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetCurrentUserQueryHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<User?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var externalSubject = _currentUserService.ExternalSubject;
            if (string.IsNullOrWhiteSpace(externalSubject))
            {
                return null;
            }

            return await _userRepository.GetByExternalSubjectAsync(externalSubject, "google", cancellationToken);
        }
    }
}
