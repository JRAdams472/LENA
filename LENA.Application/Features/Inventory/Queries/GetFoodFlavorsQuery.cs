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
    public record GetFoodFlavorsQuery : IRequest<IReadOnlyList<FoodFlavor>>;

        public class GetFoodFlavorsQueryHandler : IRequestHandler<GetFoodFlavorsQuery, IReadOnlyList<FoodFlavor>>
        {
            private readonly IFoodFlavorRepository _foodFlavorRepository;
            public GetFoodFlavorsQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
            public async Task<IReadOnlyList<FoodFlavor>> Handle(GetFoodFlavorsQuery request, CancellationToken cancellationToken)
                => await _foodFlavorRepository.ListAllAsync();
        }
}