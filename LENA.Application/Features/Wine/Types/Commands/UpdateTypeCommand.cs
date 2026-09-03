using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Commands
{
    public record UpdateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => Type;
    }

    public class UpdateTypeCommandHandler : IRequestHandler<UpdateTypeCommand, TypeEntity>
    {
        private readonly ITypeRepository _typeRepository;

        private readonly IMemoryCache _cache;
        public UpdateTypeCommandHandler(ITypeRepository typeRepository, IMemoryCache cache)
        {
            _typeRepository = typeRepository;
            _cache = cache;
        }
        public async Task<TypeEntity> Handle(UpdateTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await _typeRepository.UpdateAsync(request.Type, cancellationToken);
            _cache.Remove(CacheKeys.Types);
            return result;
        }
    }
}