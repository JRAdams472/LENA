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
    public record UpdateNutrientTypeCommand(NutrientType NutrientType) : IRequest<NutrientType>;

        public class UpdateNutrientTypeCommandHandler : IRequestHandler<UpdateNutrientTypeCommand, NutrientType>
        {
            private readonly INutrientTypeRepository _nutrientTypeRepository;
            public UpdateNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
            public async Task<NutrientType> Handle(UpdateNutrientTypeCommand request, CancellationToken cancellationToken)
                => await _nutrientTypeRepository.UpdateAsync(request.NutrientType);
        }
}