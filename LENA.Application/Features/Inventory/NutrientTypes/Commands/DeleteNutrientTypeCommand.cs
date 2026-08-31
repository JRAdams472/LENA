using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.NutrientTypes.Commands
{
    public record DeleteNutrientTypeCommand(int NutrientTypeId) : IRequest<NutrientType?>;

        public class DeleteNutrientTypeCommandHandler : IRequestHandler<DeleteNutrientTypeCommand, NutrientType?>
        {
            private readonly INutrientTypeRepository _nutrientTypeRepository;
            public DeleteNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
            public async Task<NutrientType?> Handle(DeleteNutrientTypeCommand request, CancellationToken cancellationToken)
            {
                var nutrientType = await _nutrientTypeRepository.GetByIdAsync(request.NutrientTypeId, cancellationToken);
                if (nutrientType == null)
                    return null;
    
                return await _nutrientTypeRepository.DeleteAsync(nutrientType, cancellationToken);
            }
        }
}