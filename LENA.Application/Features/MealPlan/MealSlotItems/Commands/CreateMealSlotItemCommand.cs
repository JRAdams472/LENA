using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;

using MediatR;

using MealSlotItem = LENA.Domain.Entity.MealPlan.MealSlotItem;

namespace LENA.Application.Features.MealPlan.MealSlotItems.Commands
{
    public record CreateMealSlotItemCommand(MealSlotItem MealSlotItem) : IRequest<MealSlotItem>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => MealSlotItem;
    }

    public class CreateMealSlotItemCommandHandler : IRequestHandler<CreateMealSlotItemCommand, MealSlotItem>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public CreateMealSlotItemCommandHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealSlotItem> Handle(CreateMealSlotItemCommand request, CancellationToken cancellationToken)
        {
            return await _mealPlanRepository.AddSlotItemAsync(request.MealSlotItem, cancellationToken);
        }
    }
}