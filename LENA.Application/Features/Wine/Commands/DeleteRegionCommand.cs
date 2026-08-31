using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Commands
{
    public record DeleteRegionCommand(int RegionId) : IRequest<Region?>;

        public class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, Region?>
        {
            private readonly IRegionRepository _regionRepository;
            public DeleteRegionCommandHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
            public async Task<Region?> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
            {
                var region = await _regionRepository.GetByIdAsync(request.RegionId);
                if (region == null)
                    return null;
    
                return await _regionRepository.DeleteAsync(region);
            }
        }
}