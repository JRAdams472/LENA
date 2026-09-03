using LENA.Application.Contracts.Persistence;

using MediatR;

using MealSlotItem = LENA.Domain.Entity.MealPlan.MealSlotItem;

namespace LENA.Application.Features.MealPlan.MealSlotItems.Queries
{
    public record GetMealSlotItemsBySlotIdQuery(int MealSlotId) : IRequest<IReadOnlyList<MealSlotItem>>;

    public class GetMealSlotItemsBySlotIdQueryHandler : IRequestHandler<GetMealSlotItemsBySlotIdQuery, IReadOnlyList<MealSlotItem>>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public GetMealSlotItemsBySlotIdQueryHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<IReadOnlyList<MealSlotItem>> Handle(GetMealSlotItemsBySlotIdQuery request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.GetSlotItemsBySlotIdAsync(request.MealSlotId, cancellationToken);
        }
    }
}