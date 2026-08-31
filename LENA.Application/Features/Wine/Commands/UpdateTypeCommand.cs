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
    public record UpdateTypeCommand(TypeEntity Type) : IRequest<TypeEntity>;

        public class UpdateTypeCommandHandler : IRequestHandler<UpdateTypeCommand, TypeEntity>
        {
            private readonly ITypeRepository _typeRepository;
            public UpdateTypeCommandHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
            public async Task<TypeEntity> Handle(UpdateTypeCommand request, CancellationToken cancellationToken)
                => await _typeRepository.UpdateAsync(request.Type);
        }
}