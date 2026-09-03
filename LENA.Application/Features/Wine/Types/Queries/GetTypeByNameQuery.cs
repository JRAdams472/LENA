using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;

using MediatR;

using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Queries
{
    public record GetTypeByNameQuery(string Name) : IRequest<TypeEntity?>;

    public class GetTypeByNameQueryHandler : IRequestHandler<GetTypeByNameQuery, TypeEntity?>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypeByNameQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity?> Handle(GetTypeByNameQuery request, CancellationToken cancellationToken)
            => await _typeRepository.GetByNameAsync(request.Name, cancellationToken) ?? throw new NotFoundException(nameof(TypeEntity), request.Name);
    }
}