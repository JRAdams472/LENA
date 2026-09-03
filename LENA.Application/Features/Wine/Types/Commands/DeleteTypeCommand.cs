using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Commands
{
    public record DeleteTypeCommand(int TypeId) : IRequest<TypeEntity?>;

    public class DeleteTypeCommandHandler : IRequestHandler<DeleteTypeCommand, TypeEntity?>
    {
        private readonly ITypeRepository _typeRepository;

        private readonly IMemoryCache _cache;
        public DeleteTypeCommandHandler(ITypeRepository typeRepository, IMemoryCache cache)
        {
            _typeRepository = typeRepository;
            _cache = cache;
        }
        public async Task<TypeEntity?> Handle(DeleteTypeCommand request, CancellationToken cancellationToken)
        {
            var type = await _typeRepository.GetByIdAsync(request.TypeId, cancellationToken) ?? throw new NotFoundException(nameof(TypeEntity), request.TypeId);

            var result = await _typeRepository.DeleteAsync(type, cancellationToken);
            _cache.Remove(CacheKeys.Types);
            return result;
        }
    }
}