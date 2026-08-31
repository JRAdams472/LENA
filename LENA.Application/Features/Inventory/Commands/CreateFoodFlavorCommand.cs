using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Inventory.Commands
{
    public record CreateFoodFlavorCommand(FoodFlavor FoodFlavor) : IRequest<FoodFlavor>;

        public class CreateFoodFlavorCommandHandler : IRequestHandler<CreateFoodFlavorCommand, FoodFlavor>
        {
            private readonly IFoodFlavorRepository _foodFlavorRepository;
            public CreateFoodFlavorCommandHandler(IFoodFlavorRepository foodFlavorRepository) => _foodFlavorRepository = foodFlavorRepository;
            public async Task<FoodFlavor> Handle(CreateFoodFlavorCommand request, CancellationToken cancellationToken)
                => await _foodFlavorRepository.CreateAsync(request.FoodFlavor);
        }
}