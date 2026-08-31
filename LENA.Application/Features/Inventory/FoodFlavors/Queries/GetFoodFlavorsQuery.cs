using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodFlavors.Queries
{
    public record GetFoodFlavorsQuery : IRequest<IReadOnlyList<FoodFlavor>>;

        public class GetFoodFlavorsQueryHandler : IRequestHandler<GetFoodFlavorsQuery, IReadOnlyList<FoodFlavor>>
        {
            private readonly IFoodFlavorRepository _foodFlavorRepository;
            public GetFoodFlavorsQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
            public async Task<IReadOnlyList<FoodFlavor>> Handle(GetFoodFlavorsQuery request, CancellationToken cancellationToken)
                => await _foodFlavorRepository.ListAllAsync();
        }
}