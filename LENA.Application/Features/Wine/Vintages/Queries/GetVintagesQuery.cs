using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Queries
{
    public record GetVintagesQuery : IRequest<IReadOnlyList<Vintage>>;

    public class GetVintagesQueryHandler : IRequestHandler<GetVintagesQuery, IReadOnlyList<Vintage>>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintagesQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<IReadOnlyList<Vintage>> Handle(GetVintagesQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.ListAllAsync(cancellationToken);
    }
}
