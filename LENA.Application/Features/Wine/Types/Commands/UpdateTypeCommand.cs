using LENA.Application.Contracts.Persistence;
using MediatR;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Commands
{
    public record UpdateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>;

        public class UpdateTypeCommandHandler : IRequestHandler<UpdateTypeCommand, TypeEntity>
        {
            private readonly ITypeRepository _typeRepository;
            public UpdateTypeCommandHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
            public async Task<TypeEntity> Handle(UpdateTypeCommand request, CancellationToken cancellationToken)
                => await _typeRepository.UpdateAsync(request.Type, cancellationToken);
        }
}