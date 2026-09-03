using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Wine;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Features.Wine.Vintages.Commands
{
    public record UpdateVintageCommand(Vintage Vintage) : IRequest<Vintage>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => Vintage;
    }

    public class UpdateVintageCommandHandler : IRequestHandler<UpdateVintageCommand, Vintage>
    {
        private readonly IVintageRepository _vintageRepository;

        private readonly IMemoryCache _cache;
        public UpdateVintageCommandHandler(IVintageRepository vintageRepository, IMemoryCache cache)
        {
            _vintageRepository = vintageRepository;
            _cache = cache;
        }
        public async Task<Vintage> Handle(UpdateVintageCommand request, CancellationToken cancellationToken)
        {
            var result = await _vintageRepository.UpdateAsync(request.Vintage, cancellationToken);
            _cache.Remove(CacheKeys.Vintages);
            _cache.Remove(CacheKeys.ActiveVintages);
            return result;
        }
    }
}