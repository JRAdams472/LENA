using LENA.Domain.Entity.Common;
using LENA.Application.Contracts.Auditing;
using MediatR;

namespace LENA.Application.Behaviors
{
    public class AuditingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUser;
        private readonly TimeProvider _timeProvider;

        public AuditingBehavior(ICurrentUserService currentUser, TimeProvider timeProvider)
        {
            _currentUser = currentUser;
            _timeProvider = timeProvider;
        }

        public Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var now = _timeProvider.GetUtcNow().UtcDateTime;

            switch (request)
            {
                case ICreateCommand create:
                    create.AuditableEntity.CreatedBy = _currentUser.UserName;
                    create.AuditableEntity.CreateDate = now;
                    create.AuditableEntity.LastUpdatedBy = null;
                    create.AuditableEntity.LastUpdatedDate = null;
                    break;
                case IUpdateCommand update:
                    update.AuditableEntity.LastUpdatedBy = _currentUser.UserName;
                    update.AuditableEntity.LastUpdatedDate = now;
                    break;
            }

            return next();
        }
    }
}
