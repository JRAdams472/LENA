using LENA.Application.Contracts.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TypeEntity = LENA.Domain.Entity.Wine.Type;
using LENA.Domain.Entity.Wine;
using System;
using System.Linq;

namespace LENA.Application.Features.Wine.Commands
{
    public record CreateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>;

        public class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, TypeEntity>
        {
            private readonly ITypeRepository _typeRepository;
            public CreateTypeCommandHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
            public async Task<TypeEntity> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
                => await _typeRepository.CreateAsync(request.Type);
        }
}