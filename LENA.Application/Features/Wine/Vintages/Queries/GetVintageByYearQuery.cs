using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Queries
{
    public record GetVintageByYearQuery(int Year) : IRequest<Vintage?>;

    public class GetVintageByYearQueryHandler : IRequestHandler<GetVintageByYearQuery, Vintage?>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintageByYearQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage?> Handle(GetVintageByYearQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.GetByYearAsync(request.Year, cancellationToken);
    }
}
