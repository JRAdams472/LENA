using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Inventory.Queries
{
    public record GetFoodNutrientByIdQuery(int FoodNutrientId) : IRequest<FoodNutrient?>;

        public class GetFoodNutrientByIdQueryHandler : IRequestHandler<GetFoodNutrientByIdQuery, FoodNutrient?>
        {
            private readonly IFoodNutrientRepository _foodNutrientRepository;
            public GetFoodNutrientByIdQueryHandler(IFoodNutrientRepository foodNutrientRepository) => _foodNutrientRepository = foodNutrientRepository;
            public async Task<FoodNutrient?> Handle(GetFoodNutrientByIdQuery request, CancellationToken cancellationToken)
                => await _foodNutrientRepository.GetByIdAsync(request.FoodNutrientId);
        }
}