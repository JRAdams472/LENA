using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Regions.Commands
{
    public record CreateRegionCommand(Region Region) : IRequest<Region>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Region;
    }

    public class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, Region>
    {
        private readonly IRegionRepository _regionRepository;
        public CreateRegionCommandHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<Region> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
            => await _regionRepository.CreateAsync(request.Region, cancellationToken);
    }
}
