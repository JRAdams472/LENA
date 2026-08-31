using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using MediatR;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Commands
{
    public record CreateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Type;
    }

    public class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, TypeEntity>
    {
        private readonly ITypeRepository _typeRepository;
        public CreateTypeCommandHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
            => await _typeRepository.CreateAsync(request.Type, cancellationToken);
    }
}
