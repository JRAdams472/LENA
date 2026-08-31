using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record SearchBottlesQuery(string SearchTerm) : IRequest<IReadOnlyList<Bottle>>;

    public class SearchBottlesQueryHandler : IRequestHandler<SearchBottlesQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public SearchBottlesQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<IReadOnlyList<Bottle>> Handle(SearchBottlesQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.SearchBottlesAsync(request.SearchTerm, cancellationToken);
    }
}
