using LENA.Application.Contracts.Persistence;
using MediatR;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Queries
{
    public record GetTypesQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<TypeEntity>>;

    public class GetTypesQueryHandler : IRequestHandler<GetTypesQuery, LENA.Application.Models.PagedResult<TypeEntity>>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypesQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<LENA.Application.Models.PagedResult<TypeEntity>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
            => await _typeRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
