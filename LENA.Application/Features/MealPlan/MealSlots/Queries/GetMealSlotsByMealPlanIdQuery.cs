using LENA.Application.Contracts.Persistence;
using MealSlot = LENA.Domain.Entity.MealPlan.MealSlot;
using MediatR;

namespace LENA.Application.Features.MealPlan.MealSlots.Queries
{
    public record GetMealSlotsByMealPlanIdQuery(int MealPlanId) : IRequest<IReadOnlyList<MealSlot>>;

    public class GetMealSlotsByMealPlanIdQueryHandler : IRequestHandler<GetMealSlotsByMealPlanIdQuery, IReadOnlyList<MealSlot>>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public GetMealSlotsByMealPlanIdQueryHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<IReadOnlyList<MealSlot>> Handle(GetMealSlotsByMealPlanIdQuery request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.GetSlotsByMealPlanIdAsync(request.MealPlanId, cancellationToken);
        }
    }
}
