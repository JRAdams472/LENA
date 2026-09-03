using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.FoodFlavors.Commands
{
    public record CreateFoodFlavorCommand(FoodFlavor FoodFlavor) : IRequest<FoodFlavor>;

    public class CreateFoodFlavorCommandHandler : IRequestHandler<CreateFoodFlavorCommand, FoodFlavor>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public CreateFoodFlavorCommandHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor> Handle(CreateFoodFlavorCommand request, CancellationToken cancellationToken)
            => await _foodFlavorRepository.CreateAsync(request.FoodFlavor, cancellationToken);
    }
}