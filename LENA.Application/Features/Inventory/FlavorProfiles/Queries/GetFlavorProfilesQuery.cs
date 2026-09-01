using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Queries
{
    public record GetFlavorProfilesQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<FlavorProfile>>;

    public class GetFlavorProfilesQueryHandler : IRequestHandler<GetFlavorProfilesQuery, LENA.Application.Models.PagedResult<FlavorProfile>>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public GetFlavorProfilesQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<LENA.Application.Models.PagedResult<FlavorProfile>> Handle(GetFlavorProfilesQuery request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.ListAllAsync(request.Paging, cancellationToken);
    }
}
