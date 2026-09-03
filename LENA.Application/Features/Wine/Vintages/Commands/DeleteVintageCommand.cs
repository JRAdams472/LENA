using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Wine;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace LENA.Application.Features.Wine.Vintages.Commands
{
    public record DeleteVintageCommand(int VintageId) : IRequest<Vintage?>;

    public class DeleteVintageCommandHandler : IRequestHandler<DeleteVintageCommand, Vintage?>
    {
        private readonly IVintageRepository _vintageRepository;

        private readonly IMemoryCache _cache;
        public DeleteVintageCommandHandler(IVintageRepository vintageRepository, IMemoryCache cache)
        {
            _vintageRepository = vintageRepository;
            _cache = cache;
        }
        public async Task<Vintage?> Handle(DeleteVintageCommand request, CancellationToken cancellationToken)
        {
            var vintage = await _vintageRepository.GetByIdAsync(request.VintageId, cancellationToken) ?? throw new NotFoundException(nameof(Vintage), request.VintageId);

            var result = await _vintageRepository.DeleteAsync(vintage, cancellationToken);
            _cache.Remove(CacheKeys.Vintages);
            _cache.Remove(CacheKeys.ActiveVintages);
            return result;
        }
    }
}