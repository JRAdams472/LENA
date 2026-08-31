using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Queries
{
    public record GetActiveVintagesQuery : IRequest<IReadOnlyList<Vintage>>;

        public class GetActiveVintagesQueryHandler : IRequestHandler<GetActiveVintagesQuery, IReadOnlyList<Vintage>>
        {
            private readonly IVintageRepository _vintageRepository;
            public GetActiveVintagesQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
            public async Task<IReadOnlyList<Vintage>> Handle(GetActiveVintagesQuery request, CancellationToken cancellationToken)
                => await _vintageRepository.GetAllActiveAsync();
        }
}