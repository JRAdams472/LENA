using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using MealSlot = LENA.Domain.Entity.MealPlan.MealSlot;
using MediatR;

namespace LENA.Application.Features.MealPlan.MealSlots.Commands
{
    public record UpdateMealSlotCommand(MealSlot MealSlot) : IRequest<MealSlot>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => MealSlot;
    }

    public class UpdateMealSlotCommandHandler : IRequestHandler<UpdateMealSlotCommand, MealSlot>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public UpdateMealSlotCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealSlot> Handle(UpdateMealSlotCommand request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.UpdateSlotAsync(request.MealSlot, cancellationToken);
        }
    }
}
