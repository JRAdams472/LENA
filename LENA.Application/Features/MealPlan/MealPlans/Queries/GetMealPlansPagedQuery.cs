using LENA.Application.Contracts.Persistence;

using MediatR;

using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.Application.Features.MealPlan.MealPlans.Queries
{
    public record GetMealPlansPagedQuery(int PageNumber, int PageSize) : IRequest<LENA.Application.Models.PagedResult<MealPlanEntity>>;

    public class GetMealPlansPagedQueryHandler : IRequestHandler<GetMealPlansPagedQuery, LENA.Application.Models.PagedResult<MealPlanEntity>>
    {
        private readonly IMealPlanRepository _mealPlanRepository;
        public GetMealPlansPagedQueryHandler(IMealPlanRepository mealPlanRepository) => _mealPlanRepository = mealPlanRepository;
        public async Task<LENA.Application.Models.PagedResult<MealPlanEntity>> Handle(GetMealPlansPagedQuery request, CancellationToken cancellationToken)
            => await _mealPlanRepository.ListPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}