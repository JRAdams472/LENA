using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.NutrientTypes.Commands
{
    public record UpdateNutrientTypeCommand(NutrientType NutrientType) : IRequest<NutrientType>;

        public class UpdateNutrientTypeCommandHandler : IRequestHandler<UpdateNutrientTypeCommand, NutrientType>
        {
            private readonly INutrientTypeRepository _nutrientTypeRepository;
            public UpdateNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
            public async Task<NutrientType> Handle(UpdateNutrientTypeCommand request, CancellationToken cancellationToken)
                => await _nutrientTypeRepository.UpdateAsync(request.NutrientType, cancellationToken);
        }
}