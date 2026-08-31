using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Inventory
{
    // Queries
    public record GetNutrientTypeByIdQuery(int NutrientTypeId) : IRequest<NutrientType?>;
    public record GetNutrientTypesQuery : IRequest<IReadOnlyList<NutrientType>>;
    public record GetNutrientTypeByNameQuery(string Name) : IRequest<NutrientType?>;

    // Commands
    public record CreateNutrientTypeCommand(NutrientType NutrientType) : IRequest<NutrientType>;
    public record UpdateNutrientTypeCommand(NutrientType NutrientType) : IRequest<NutrientType>;
    public record DeleteNutrientTypeCommand(int NutrientTypeId) : IRequest<NutrientType?>;

    // Handlers
    public class GetNutrientTypeByIdQueryHandler : IRequestHandler<GetNutrientTypeByIdQuery, NutrientType?>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public GetNutrientTypeByIdQueryHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<NutrientType?> Handle(GetNutrientTypeByIdQuery request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.GetByIdAsync(request.NutrientTypeId);
    }

    public class GetNutrientTypesQueryHandler : IRequestHandler<GetNutrientTypesQuery, IReadOnlyList<NutrientType>>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public GetNutrientTypesQueryHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<IReadOnlyList<NutrientType>> Handle(GetNutrientTypesQuery request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.ListAllAsync();
    }

    public class GetNutrientTypeByNameQueryHandler : IRequestHandler<GetNutrientTypeByNameQuery, NutrientType?>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public GetNutrientTypeByNameQueryHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<NutrientType?> Handle(GetNutrientTypeByNameQuery request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.GetByNameAsync(request.Name);
    }

    public class CreateNutrientTypeCommandHandler : IRequestHandler<CreateNutrientTypeCommand, NutrientType>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public CreateNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<NutrientType> Handle(CreateNutrientTypeCommand request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.CreateAsync(request.NutrientType);
    }

    public class UpdateNutrientTypeCommandHandler : IRequestHandler<UpdateNutrientTypeCommand, NutrientType>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public UpdateNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<NutrientType> Handle(UpdateNutrientTypeCommand request, CancellationToken cancellationToken)
            => await _nutrientTypeRepository.UpdateAsync(request.NutrientType);
    }

    public class DeleteNutrientTypeCommandHandler : IRequestHandler<DeleteNutrientTypeCommand, NutrientType?>
    {
        private readonly INutrientTypeRepository _nutrientTypeRepository;
        public DeleteNutrientTypeCommandHandler(INutrientTypeRepository nutrientTypeRepository) => _nutrientTypeRepository = nutrientTypeRepository;
        public async Task<NutrientType?> Handle(DeleteNutrientTypeCommand request, CancellationToken cancellationToken)
        {
            var nutrientType = await _nutrientTypeRepository.GetByIdAsync(request.NutrientTypeId);
            if (nutrientType == null)
                return null;

            return await _nutrientTypeRepository.DeleteAsync(nutrientType);
        }
    }
}
