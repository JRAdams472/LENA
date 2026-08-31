using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FoodNutrients.Queries
{
    public record GetFoodNutrientsByNutrientIdQuery(int NutrientId) : IRequest<IEnumerable<FoodNutrient>>;

        public class GetFoodNutrientsByNutrientIdQueryHandler : IRequestHandler<GetFoodNutrientsByNutrientIdQuery, IEnumerable<FoodNutrient>>
        {
            private readonly IFoodNutrientRepository _foodNutrientRepository;
            public GetFoodNutrientsByNutrientIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
            public async Task<IEnumerable<FoodNutrient>> Handle(GetFoodNutrientsByNutrientIdQuery request, CancellationToken cancellationToken)
                => await _foodNutrientRepository.GetByNutrientIdAsync(request.NutrientId);
        }
}