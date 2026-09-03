using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;

using MediatR;

using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.Application.Features.MealPlan.MealPlans.Commands
{
    public record CreateMealPlanCommand(MealPlanEntity MealPlan) : IRequest<MealPlanEntity>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => MealPlan;
    }

    public class CreateMealPlanCommandHandler : IRequestHandler<CreateMealPlanCommand, MealPlanEntity>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public CreateMealPlanCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealPlanEntity> Handle(CreateMealPlanCommand request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.CreateAsync(request.MealPlan, cancellationToken);
        }
    }
}