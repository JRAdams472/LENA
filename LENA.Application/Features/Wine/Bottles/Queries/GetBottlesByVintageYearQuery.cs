using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetBottlesByVintageYearQuery(int VintageYear) : IRequest<IReadOnlyList<Bottle>>;

        public class GetBottlesByVintageYearQueryHandler : IRequestHandler<GetBottlesByVintageYearQuery, IReadOnlyList<Bottle>>
        {
            private readonly IBottleRepository _bottleRepository;
            public GetBottlesByVintageYearQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
            public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByVintageYearQuery request, CancellationToken cancellationToken)
                => await _bottleRepository.GetAllByVintageYearAsync(request.VintageYear, cancellationToken);
        }
}