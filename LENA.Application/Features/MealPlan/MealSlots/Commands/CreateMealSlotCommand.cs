using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using MealSlot = LENA.Domain.Entity.MealPlan.MealSlot;
using MediatR;

namespace LENA.Application.Features.MealPlan.MealSlots.Commands
{
    public record CreateMealSlotCommand(MealSlot MealSlot) : IRequest<MealSlot>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => MealSlot;
    }

    public class CreateMealSlotCommandHandler : IRequestHandler<CreateMealSlotCommand, MealSlot>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public CreateMealSlotCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealSlot> Handle(CreateMealSlotCommand request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.AddSlotAsync(request.MealSlot, cancellationToken);
        }
    }
}
