using System.Collections.Generic;
using LENA.Application.Contracts.Persistence;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;
using MealSlot = LENA.Domain.Entity.MealPlan.MealSlot;
using MealSlotItem = LENA.Domain.Entity.MealPlan.MealSlotItem;
using MediatR;

namespace LENA.Application.Features.MealPlan.MealPlans.Queries
{
    public record GetMealPlanByIdQuery(int MealPlanId) : IRequest<MealPlanEntity?>;

    public class GetMealPlanByIdQueryHandler : IRequestHandler<GetMealPlanByIdQuery, MealPlanEntity?>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public GetMealPlanByIdQueryHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealPlanEntity?> Handle(GetMealPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var mealPlan = await _mealPlanRepository.GetByIdAsync(request.MealPlanId, cancellationToken);
            if (mealPlan == null)
                return null;

            var slots = await _mealPlanRepository.GetSlotsByMealPlanIdAsync(request.MealPlanId, cancellationToken);
            foreach (var slot in slots)
            {
                var items = await _mealPlanRepository.GetSlotItemsBySlotIdAsync(slot.MealSlotID, cancellationToken);
                slot.MealSlotItems = new List<MealSlotItem>(items);
            }

            mealPlan.MealSlots = new List<MealSlot>(slots);

            return mealPlan;
        }
    }
}
