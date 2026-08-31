using LENA.Application.Contracts.Persistence;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;
using MediatR;

namespace LENA.Application.Features.MealPlan.MealPlans.Commands
{
    public record DeleteMealPlanCommand(int MealPlanId) : IRequest<MealPlanEntity?>;

    public class DeleteMealPlanCommandHandler : IRequestHandler<DeleteMealPlanCommand, MealPlanEntity?>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public DeleteMealPlanCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealPlanEntity?> Handle(DeleteMealPlanCommand request, CancellationToken cancellationToken)
        {
            var mealPlan = await _mealPlanRepository.GetByIdAsync(request.MealPlanId, cancellationToken);
            if (mealPlan == null)
                return null;

            return await _mealPlanRepository.DeleteAsync(mealPlan, cancellationToken);
        }
    }
}
