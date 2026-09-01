using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Grocery;
using MediatR;

namespace LENA.Application.Features.Grocery.GroceryLists.Commands
{
    public record GenerateGroceryListCommand(int? MealPlanId) : IRequest<GroceryList>, ICreateCommand
    {
        private readonly GroceryList _groceryList = new() { MealPlanID = MealPlanId };

        public AuditableEntity AuditableEntity => _groceryList;
    }

    public class GenerateGroceryListCommandHandler : IRequestHandler<GenerateGroceryListCommand, GroceryList>
    {
        private readonly IGroceryListRepository _groceryListRepository;

        public GenerateGroceryListCommandHandler(IGroceryListRepository groceryListRepository)
        {
            _groceryListRepository = groceryListRepository;
        }

        public async Task<GroceryList> Handle(GenerateGroceryListCommand request, CancellationToken cancellationToken)
        {
            var groceryList = (GroceryList)request.AuditableEntity;
            groceryList.GeneratedDate = groceryList.CreateDate;
            return await _groceryListRepository.GenerateFromMealPlanAsync(groceryList, cancellationToken);
        }
    }
}
