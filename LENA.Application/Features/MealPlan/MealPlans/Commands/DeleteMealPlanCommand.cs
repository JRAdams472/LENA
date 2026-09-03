using LENA.Application.Contracts.Persistence;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;
using MediatR;
using LENA.Application.Exceptions;

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
            var mealPlan = await _mealPlanRepository.GetByIdAsync(request.MealPlanId, cancellationToken) ?? throw new NotFoundException(nameof(MealPlanEntity), request.MealPlanId);

            return await _mealPlanRepository.DeleteAsync(mealPlan, cancellationToken);
        }
    }
}
