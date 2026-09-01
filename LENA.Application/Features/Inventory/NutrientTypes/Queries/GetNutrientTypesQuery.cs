using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.NutrientTypes.Queries
{
    public record GetNutrientTypesQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<NutrientType>>;

    public class GetNutrientTypesQueryHandler : IRequestHandler<GetNutrientTypesQuery, LENA.Application.Models.PagedResult<NutrientType>>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public GetNutrientTypesQueryHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<LENA.Application.Models.PagedResult<NutrientType>> Handle(GetNutrientTypesQuery request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
