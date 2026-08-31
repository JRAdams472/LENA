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
    public record GetFoodFlavorsByFoodIdQuery(int FoodId) : IRequest<IEnumerable<FoodFlavor>>;

        public class GetFoodFlavorsByFoodIdQueryHandler : IRequestHandler<GetFoodFlavorsByFoodIdQuery, IEnumerable<FoodFlavor>>
        {
            private readonly IFoodFlavorRepository _foodFlavorRepository;
            public GetFoodFlavorsByFoodIdQueryHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
            public async Task<IEnumerable<FoodFlavor>> Handle(GetFoodFlavorsByFoodIdQuery request, CancellationToken cancellationToken)
                => await _foodFlavorRepository.GetByFoodIdAsync(request.FoodId);
        }
}