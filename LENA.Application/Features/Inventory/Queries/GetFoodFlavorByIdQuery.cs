using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.Queries
{
    public record GetFoodFlavorByIdQuery(int FoodFlavorId) : IRequest<FoodFlavor?>;

        public class GetFoodFlavorByIdQueryHandler : IRequestHandler<GetFoodFlavorByIdQuery, FoodFlavor?>
        {
            private readonly IFoodFlavorRepository _foodFlavorRepository;
            public GetFoodFlavorByIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
            public async Task<FoodFlavor?> Handle(GetFoodFlavorByIdQuery request, CancellationToken cancellationToken)
                => await _foodFlavorRepository.GetByIdAsync(request.FoodFlavorId);
        }
}