using LENA.Application.Contracts.Persistence;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;
using MediatR;

namespace LENA.Application.Features.MealPlan.MealPlans.Queries
{
    public record GetMealPlansQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<MealPlanEntity>>;

    public class GetMealPlansQueryHandler : IRequestHandler<GetMealPlansQuery, LENA.Application.Models.PagedResult<MealPlanEntity>>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public GetMealPlansQueryHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<LENA.Application.Models.PagedResult<MealPlanEntity>> Handle(GetMealPlansQuery request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.ListAllAsync(request.Paging, cancellationToken);
        }
    }
}
