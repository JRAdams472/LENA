using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Features.Wine.Regions.Commands
{
    public record UpdateRegionCommand(Region Region) : IRequest<Region>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => Region;
    }

    public class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, Region>
    {
        private readonly IRegionRepository _regionRepository;

        private readonly IMemoryCache _cache;
        public UpdateRegionCommandHandler(IRegionRepository regionRepository, IMemoryCache cache)
        {
            _regionRepository = regionRepository;
            _cache = cache;
        }
        public async Task<Region> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
        {
            var result = await _regionRepository.UpdateAsync(request.Region, cancellationToken);
            _cache.Remove(CacheKeys.Regions);
            return result;
        }
    }
}