using LENA.Application.Contracts.Caching;
using LENA.Application.Contracts.Persistence;

using MediatR;

using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Queries
{
    public record GetTypesQuery : IRequest<IReadOnlyList<TypeEntity>>, ICacheableQuery<IReadOnlyList<TypeEntity>>
    {
        public string CacheKey => CacheKeys.Types;

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(10);
    }

    public class GetTypesQueryHandler : IRequestHandler<GetTypesQuery, IReadOnlyList<TypeEntity>>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypesQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<IReadOnlyList<TypeEntity>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
            => await _typeRepository.ListAllAsync(cancellationToken);
    }
}