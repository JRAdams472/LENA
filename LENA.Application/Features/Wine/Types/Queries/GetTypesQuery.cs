using LENA.Application.Contracts.Persistence;
using MediatR;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Queries
{
    public record GetTypesQuery : IRequest<IReadOnlyList<TypeEntity>>;

        public class GetTypesQueryHandler : IRequestHandler<GetTypesQuery, IReadOnlyList<TypeEntity>>
        {
            private readonly ITypeRepository _typeRepository;
            public GetTypesQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
            public async Task<IReadOnlyList<TypeEntity>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
                => await _typeRepository.ListAllAsync(cancellationToken);
        }
}