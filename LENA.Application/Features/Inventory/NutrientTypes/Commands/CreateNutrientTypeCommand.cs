using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.NutrientTypes.Commands
{
    public record CreateNutrientTypeCommand(NutrientType NutrientType) : IRequest<NutrientType>;

    public class CreateNutrientTypeCommandHandler : IRequestHandler<CreateNutrientTypeCommand, NutrientType>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public CreateNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<NutrientType> Handle(CreateNutrientTypeCommand request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.CreateAsync(request.NutrientType, cancellationToken);
    }
}
