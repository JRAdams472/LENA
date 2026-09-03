using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;

using MediatR;

using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.Application.Features.MealPlan.MealPlans.Commands
{
    public record UpdateMealPlanCommand(MealPlanEntity MealPlan) : IRequest<MealPlanEntity>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => MealPlan;
    }

    public class UpdateMealPlanCommandHandler : IRequestHandler<UpdateMealPlanCommand, MealPlanEntity>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public UpdateMealPlanCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealPlanEntity> Handle(UpdateMealPlanCommand request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.UpdateAsync(request.MealPlan, cancellationToken);
        }
    }
}