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
    public record GetTypeByNameQuery(string Name) : IRequest<TypeEntity?>;

        public class GetTypeByNameQueryHandler : IRequestHandler<GetTypeByNameQuery, TypeEntity?>
        {
            private readonly ITypeRepository _typeRepository;
            public GetTypeByNameQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
            public async Task<TypeEntity?> Handle(GetTypeByNameQuery request, CancellationToken cancellationToken)
                => await _typeRepository.GetByNameAsync(request.Name);
        }
}