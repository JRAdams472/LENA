using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Regions.Commands
{
    public record UpdateRegionCommand(Region Region) : IRequest<Region>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => Region;
    }

    public class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, Region>
    {
        private readonly IRegionRepository _regionRepository;
        public UpdateRegionCommandHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<Region> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
            => await _regionRepository.UpdateAsync(request.Region, cancellationToken);
    }
}
