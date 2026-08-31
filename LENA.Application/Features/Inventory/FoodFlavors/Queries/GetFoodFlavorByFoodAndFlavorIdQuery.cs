using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodFlavors.Queries
{
    public record GetFoodFlavorByFoodAndFlavorIdQuery(int FoodId, int FlavorId) : IRequest<FoodFlavor?>;

        public class GetFoodFlavorByFoodAndFlavorIdQueryHandler : IRequestHandler<GetFoodFlavorByFoodAndFlavorIdQuery, FoodFlavor?>
        {
            private readonly IFoodFlavorRepository _foodFlavorRepository;
            public GetFoodFlavorByFoodAndFlavorIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
            public async Task<FoodFlavor?> Handle(GetFoodFlavorByFoodAndFlavorIdQuery request, CancellationToken cancellationToken)
                => await _foodFlavorRepository.GetByFoodAndFlavorIdAsync(request.FoodId, request.FlavorId, cancellationToken);
        }
}