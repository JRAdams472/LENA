using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Wine;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Features.Wine.Regions.Commands
{
    public record DeleteRegionCommand(int RegionId) : IRequest<Region?>;

    public class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, Region?>
    {
        private readonly IRegionRepository _regionRepository;

        private readonly IMemoryCache _cache;
        public DeleteRegionCommandHandler(IRegionRepository regionRepository, IMemoryCache cache)
        {
            _regionRepository = regionRepository;
            _cache = cache;
        }
        public async Task<Region?> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
        {
            var region = await _regionRepository.GetByIdAsync(request.RegionId, cancellationToken) ?? throw new NotFoundException(nameof(Region), request.RegionId);

            var result = await _regionRepository.DeleteAsync(region, cancellationToken);
            _cache.Remove(CacheKeys.Regions);
            return result;
        }
    }
}