using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.MealPlan.MealSlots.Commands
{
    public record DeleteMealSlotCommand(int MealSlotId) : IRequest<Unit>;

    public class DeleteMealSlotCommandHandler : IRequestHandler<DeleteMealSlotCommand, Unit>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public DeleteMealSlotCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<Unit> Handle(DeleteMealSlotCommand request, CancellationToken cancellationToken)
        {
            await _mealPlanRepository.DeleteSlotAsync(request.MealSlotId, cancellationToken);
            return Unit.Value;
        }
    }
}
