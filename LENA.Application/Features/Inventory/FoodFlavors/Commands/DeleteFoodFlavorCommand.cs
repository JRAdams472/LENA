using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.FoodFlavors.Commands
{
    public record DeleteFoodFlavorCommand(int FoodId, int FlavorId) : IRequest<FoodFlavor?>;

    public class DeleteFoodFlavorCommandHandler : IRequestHandler<DeleteFoodFlavorCommand, FoodFlavor?>
    {
        private readonly IFoodFlavorRepository _foodFlavorRepository;
        public DeleteFoodFlavorCommandHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
        public async Task<FoodFlavor?> Handle(DeleteFoodFlavorCommand request, CancellationToken cancellationToken)
        {
            var foodFlavor = await _foodFlavorRepository.GetByFoodAndFlavorIdAsync(request.FoodId, request.FlavorId, cancellationToken) ?? throw new NotFoundException(nameof(FoodFlavor), ($"{request.FoodId}-{request.FlavorId}"));

            return await _foodFlavorRepository.DeleteAsync(foodFlavor, cancellationToken);
        }
    }
}