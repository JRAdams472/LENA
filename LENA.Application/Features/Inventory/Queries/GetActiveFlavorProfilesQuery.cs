using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LENA.Application.Features.Inventory.Queries
{
    public record GetActiveFlavorProfilesQuery : IRequest<IReadOnlyList<FlavorProfile>>;

        public class GetActiveFlavorProfilesQueryHandler : IRequestHandler<GetActiveFlavorProfilesQuery, IReadOnlyList<FlavorProfile>>
        {
            private readonly IFlavorProfileRepository _flavorProfileRepository;
            public GetActiveFlavorProfilesQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
            public async Task<IReadOnlyList<FlavorProfile>> Handle(GetActiveFlavorProfilesQuery request, CancellationToken cancellationToken)
                => await _flavorProfileRepository.GetAllActiveAsync();
        }
}