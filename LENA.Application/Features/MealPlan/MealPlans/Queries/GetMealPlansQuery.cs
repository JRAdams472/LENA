using LENA.Application.Contracts.Persistence;

using MediatR;

using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.Application.Features.MealPlan.MealPlans.Queries
{
    public record GetMealPlansQuery : IRequest<IReadOnlyList<MealPlanEntity>>;

    public class GetMealPlansQueryHandler : IRequestHandler<GetMealPlansQuery, IReadOnlyList<MealPlanEntity>>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public GetMealPlansQueryHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<IReadOnlyList<MealPlanEntity>> Handle(GetMealPlansQuery request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.ListAllAsync(cancellationToken);
        }
    }
}