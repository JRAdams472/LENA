using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Commands
{
    public record CreateBottleCommand(Bottle Bottle) : IRequest<Bottle>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Bottle;
    }

    public class CreateBottleCommandHandler : IRequestHandler<CreateBottleCommand, Bottle>
    {
        private readonly IBottleRepository _bottleRepository;

        public CreateBottleCommandHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<Bottle> Handle(CreateBottleCommand request, CancellationToken cancellationToken)
        {
            return await _bottleRepository.CreateAsync(request.Bottle, cancellationToken);
        }
    }
}
