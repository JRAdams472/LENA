using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine.Queries
{
    public record GetBottlesByCountryIdQuery(int CountryId) : IRequest<IReadOnlyList<Bottle>>;
    public record GetBottlesByRegionIdQuery(int RegionId) : IRequest<IReadOnlyList<Bottle>>;
    public record GetBottlesByTypeIdQuery(int TypeId) : IRequest<IReadOnlyList<Bottle>>;
    public record GetBottlesByVintageYearQuery(int VintageYear) : IRequest<IReadOnlyList<Bottle>>;
    public record GetFavoriteBottlesQuery : IRequest<IReadOnlyList<Bottle>>;
    public record SearchBottlesQuery(string SearchTerm) : IRequest<IReadOnlyList<Bottle>>;
    public record GetTotalBottleCountQuery : IRequest<int>;

    public class GetBottlesByCountryIdQueryHandler : IRequestHandler<GetBottlesByCountryIdQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetBottlesByCountryIdQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByCountryIdQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.GetAllByCountryIdAsync(request.CountryId);
    }

    public class GetBottlesByRegionIdQueryHandler : IRequestHandler<GetBottlesByRegionIdQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetBottlesByRegionIdQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByRegionIdQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.GetAllByRegionIdAsync(request.RegionId);
    }

    public class GetBottlesByTypeIdQueryHandler : IRequestHandler<GetBottlesByTypeIdQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetBottlesByTypeIdQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByTypeIdQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.GetAllByTypeIdAsync(request.TypeId);
    }

    public class GetBottlesByVintageYearQueryHandler : IRequestHandler<GetBottlesByVintageYearQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetBottlesByVintageYearQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesByVintageYearQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.GetAllByVintageYearAsync(request.VintageYear);
    }

    public class GetFavoriteBottlesQueryHandler : IRequestHandler<GetFavoriteBottlesQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetFavoriteBottlesQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<IReadOnlyList<Bottle>> Handle(GetFavoriteBottlesQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.GetFavoritesAsync();
    }

    public class SearchBottlesQueryHandler : IRequestHandler<SearchBottlesQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;
        public SearchBottlesQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<IReadOnlyList<Bottle>> Handle(SearchBottlesQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.SearchBottlesAsync(request.SearchTerm);
    }

    public class GetTotalBottleCountQueryHandler : IRequestHandler<GetTotalBottleCountQuery, int>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetTotalBottleCountQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<int> Handle(GetTotalBottleCountQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.GetTotalBottleCountAsync();
    }
}
