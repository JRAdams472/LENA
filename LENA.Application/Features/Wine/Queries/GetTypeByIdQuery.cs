using LENA.Application.Contracts.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TypeEntity = LENA.Domain.Entity.Wine.Type;
using LENA.Domain.Entity.Wine;
using System;
using System.Linq;

namespace LENA.Application.Features.Wine.Queries
{
    public record GetTypeByIdQuery(int TypeId) : IRequest<TypeEntity?>;

        public class GetTypeByIdQueryHandler : IRequestHandler<GetTypeByIdQuery, TypeEntity?>
        {
            private readonly ITypeRepository _typeRepository;
            public GetTypeByIdQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
            public async Task<TypeEntity?> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
                => await _typeRepository.GetByIdAsync(request.TypeId);
        }
}