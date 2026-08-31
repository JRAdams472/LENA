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
    public record GetFlavorProfilesQuery : IRequest<IReadOnlyList<FlavorProfile>>;

        public class GetFlavorProfilesQueryHandler : IRequestHandler<GetFlavorProfilesQuery, IReadOnlyList<FlavorProfile>>
        {
            private readonly IFlavorProfileRepository _flavorProfileRepository;
            public GetFlavorProfilesQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
            public async Task<IReadOnlyList<FlavorProfile>> Handle(GetFlavorProfilesQuery request, CancellationToken cancellationToken)
                => await _flavorProfileRepository.ListAllAsync();
        }
}