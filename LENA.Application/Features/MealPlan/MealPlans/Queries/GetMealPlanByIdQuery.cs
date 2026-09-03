using System.Collections.Generic;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;

using MediatR;

using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;
using MealSlot = LENA.Domain.Entity.MealPlan.MealSlot;
using MealSlotItem = LENA.Domain.Entity.MealPlan.MealSlotItem;

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
            var mealPlan = await _mealPlanRepository.GetByIdAsync(request.MealPlanId, cancellationToken) ?? throw new NotFoundException(nameof(MealPlanEntity), request.MealPlanId);

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