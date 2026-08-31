using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LENA.Application.Features.Inventory.Commands
{
    public record CreateNutrientTypeCommand(NutrientType NutrientType) : IRequest<NutrientType>;

        public class CreateNutrientTypeCommandHandler : IRequestHandler<CreateNutrientTypeCommand, NutrientType>
        {
            private readonly INutrientTypeRepository _nutrientTypeRepository;
            public CreateNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
            public async Task<NutrientType> Handle(CreateNutrientTypeCommand request, CancellationToken cancellationToken)
                => await _nutrientTypeRepository.CreateAsync(request.NutrientType);
        }
}