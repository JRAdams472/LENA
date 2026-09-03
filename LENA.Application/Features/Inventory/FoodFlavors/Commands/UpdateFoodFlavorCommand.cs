using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.FoodFlavors.Commands
{
    public record UpdateFoodFlavorCommand(FoodFlavor FoodFlavor) : IRequest<FoodFlavor>;

    public class UpdateFoodFlavorCommandHandler : IRequestHandler<UpdateFoodFlavorCommand, FoodFlavor>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public UpdateFoodFlavorCommandHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor> Handle(UpdateFoodFlavorCommand request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.UpdateAsync(request.FoodFlavor, cancellationToken);
    }
}