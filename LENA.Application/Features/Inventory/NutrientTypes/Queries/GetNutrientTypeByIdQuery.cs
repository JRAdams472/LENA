using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.NutrientTypes.Queries
{
    public record GetNutrientTypeByIdQuery(int NutrientTypeId) : IRequest<NutrientType?>;

    public class GetNutrientTypeByIdQueryHandler : IRequestHandler<GetNutrientTypeByIdQuery, NutrientType?>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public GetNutrientTypeByIdQueryHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<NutrientType?> Handle(GetNutrientTypeByIdQuery request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.GetByIdAsync(request.NutrientTypeId, cancellationToken);
    }
}
