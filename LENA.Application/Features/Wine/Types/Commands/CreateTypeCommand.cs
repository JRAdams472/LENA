using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Commands
{
    public record CreateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Type;
    }

    public class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, TypeEntity>
    {
        private readonly ITypeRepository _typeRepository;

        private readonly IMemoryCache _cache;
        public CreateTypeCommandHandler(ITypeRepository typeRepository, IMemoryCache cache)
        {
            _typeRepository = typeRepository;
            _cache = cache;
        }
        public async Task<TypeEntity> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await _typeRepository.CreateAsync(request.Type, cancellationToken);
            _cache.Remove(CacheKeys.Types);
            return result;
        }
    }
}