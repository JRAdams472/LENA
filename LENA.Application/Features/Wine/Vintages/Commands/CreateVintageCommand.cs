using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Commands
{
    public record CreateVintageCommand(Vintage Vintage) : IRequest<Vintage>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Vintage;
    }

    public class CreateVintageCommandHandler : IRequestHandler<CreateVintageCommand, Vintage>
    {
        private readonly IVintageRepository _vintageRepository;
        public CreateVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage> Handle(CreateVintageCommand request, CancellationToken cancellationToken)
            => await _vintageRepository.CreateAsync(request.Vintage, cancellationToken);
    }
}
