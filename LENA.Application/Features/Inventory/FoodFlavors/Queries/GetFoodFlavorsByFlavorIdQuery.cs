using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodFlavors.Queries
{
    public record GetFoodFlavorsByFlavorIdQuery(int FlavorId) : IRequest<IEnumerable<FoodFlavor>>;

    public class GetFoodFlavorsByFlavorIdQueryHandler : IRequestHandler<GetFoodFlavorsByFlavorIdQuery, IEnumerable<FoodFlavor>>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public GetFoodFlavorsByFlavorIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<IEnumerable<FoodFlavor>> Handle(GetFoodFlavorsByFlavorIdQuery request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.GetByFlavorIdAsync(request.FlavorId, cancellationToken);
    }
}
