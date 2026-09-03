using LENA.Application.Contracts.Persistence;

using MediatR;

namespace LENA.Application.Features.MealPlan.MealSlotItems.Commands
{
    public record DeleteMealSlotItemCommand(int MealSlotItemId) : IRequest<Unit>;

    public class DeleteMealSlotItemCommandHandler : IRequestHandler<DeleteMealSlotItemCommand, Unit>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public DeleteMealSlotItemCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<Unit> Handle(DeleteMealSlotItemCommand request, CancellationToken cancellationToken)
        {
            await _mealPlanRepository.DeleteSlotItemAsync(request.MealSlotItemId, cancellationToken);
            return Unit.Value;
        }
    }
}