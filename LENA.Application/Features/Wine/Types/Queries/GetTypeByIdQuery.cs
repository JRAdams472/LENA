using LENA.Application.Contracts.Persistence;
using MediatR;
using LENA.Application.Exceptions;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Features.Wine.Types.Queries
{
    public record GetTypeByIdQuery(int TypeId) : IRequest<TypeEntity?>;

    public class GetTypeByIdQueryHandler : IRequestHandler<GetTypeByIdQuery, TypeEntity?>
    {
        private readonly ITypeRepository _typeRepository;
        public GetTypeByIdQueryHandler(ITypeRepository typeRepository) => _typeRepository = typeRepository;
        public async Task<TypeEntity?> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
            => await _typeRepository.GetByIdAsync(request.TypeId, cancellationToken) ?? throw new NotFoundException(nameof(TypeEntity), request.TypeId);
    }
}
