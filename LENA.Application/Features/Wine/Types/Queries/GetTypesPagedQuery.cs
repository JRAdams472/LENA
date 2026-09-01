using LENA.Application.Contracts.Persistence;
using MediatR;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Queries
{
    public record GetTypesPagedQuery(int PageNumber, int PageSize) : IRequest<LENA.Application.Models.PagedResult<TypeEntity>>;

    public class GetTypesPagedQueryHandler : IRequestHandler<GetTypesPagedQuery, LENA.Application.Models.PagedResult<TypeEntity>>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypesPagedQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<LENA.Application.Models.PagedResult<TypeEntity>> Handle(GetTypesPagedQuery request, CancellationToken cancellationToken)
            => await _typeRepository.ListPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
