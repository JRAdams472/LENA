using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Commands
{
    public record UpdateVintageCommand(Vintage Vintage) : IRequest<Vintage>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => Vintage;
    }

    public class UpdateVintageCommandHandler : IRequestHandler<UpdateVintageCommand, Vintage>
    {
        private readonly IVintageRepository _vintageRepository;
        public UpdateVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage> Handle(UpdateVintageCommand request, CancellationToken cancellationToken)
            => await _vintageRepository.UpdateAsync(request.Vintage, cancellationToken);
    }
}
