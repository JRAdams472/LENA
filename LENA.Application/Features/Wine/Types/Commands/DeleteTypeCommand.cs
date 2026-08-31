using LENA.Application.Contracts.Persistence;
using MediatR;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Commands
{
    public record DeleteTypeCommand(int TypeId) : IRequest<TypeEntity?>;

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