using LENA.Application.Contracts.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine
{
    // Queries
    public record GetTypeByIdQuery(int TypeId) : IRequest<TypeEntity?>;
    public record GetTypesQuery : IRequest<IReadOnlyList<TypeEntity>>;
    public record GetTypeByNameQuery(string Name) : IRequest<TypeEntity?>;

    // Commands
    public record CreateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>;
    public record UpdateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>;
    public record DeleteTypeCommand(int TypeId) : IRequest<TypeEntity?>;

    // Handlers
    public class GetTypeByIdQueryHandler : IRequestHandler<GetTypeByIdQuery, TypeEntity?>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypeByIdQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity?> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
            => await _typeRepository.GetByIdAsync(request.TypeId);
    }

    public class GetTypesQueryHandler : IRequestHandler<GetTypesQuery, IReadOnlyList<TypeEntity>>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypesQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<IReadOnlyList<TypeEntity>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
            => await _typeRepository.ListAllAsync();
    }

    public class GetTypeByNameQueryHandler : IRequestHandler<GetTypeByNameQuery, TypeEntity?>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypeByNameQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity?> Handle(GetTypeByNameQuery request, CancellationToken cancellationToken)
            => await _typeRepository.GetByNameAsync(request.Name);
    }

    public class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, TypeEntity>
    {
        private readonly ITypeRepository _typeRepository;
        public CreateTypeCommandHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
            => await _typeRepository.CreateAsync(request.Type);
    }

    public class UpdateTypeCommandHandler : IRequestHandler<UpdateTypeCommand, TypeEntity>
    {
        private readonly ITypeRepository _typeRepository;
        public UpdateTypeCommandHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity> Handle(UpdateTypeCommand request, CancellationToken cancellationToken)
            => await _typeRepository.UpdateAsync(request.Type);
    }

    public class DeleteTypeCommandHandler : IRequestHandler<DeleteTypeCommand, TypeEntity?>
    {
        private readonly ITypeRepository _typeRepository;
        public DeleteTypeCommandHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity?> Handle(DeleteTypeCommand request, CancellationToken cancellationToken)
        {
            var type = await _typeRepository.GetByIdAsync(request.TypeId);
            if (type == null)
                return null;

            return await _typeRepository.DeleteAsync(type);
        }
    }
}
