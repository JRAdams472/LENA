using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Queries
{
    public record GetNutrientTypeByNameQuery(string Name) : IRequest<NutrientType?>;

        public class GetNutrientTypeByNameQueryHandler : IRequestHandler<GetNutrientTypeByNameQuery, NutrientType?>
        {
            private readonly INutrientTypeRepository _nutrientTypeRepository;
            public GetNutrientTypeByNameQueryHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
            public async Task<NutrientType?> Handle(GetNutrientTypeByNameQuery request, CancellationToken cancellationToken)
                => await _nutrientTypeRepository.GetByNameAsync(request.Name);
        }
}