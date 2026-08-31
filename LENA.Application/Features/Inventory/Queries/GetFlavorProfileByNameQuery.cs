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
    public record GetFlavorProfileByNameQuery(string Name) : IRequest<FlavorProfile?>;

        public class GetFlavorProfileByNameQueryHandler : IRequestHandler<GetFlavorProfileByNameQuery, FlavorProfile?>
        {
            private readonly IFlavorProfileRepository _flavorProfileRepository;
            public GetFlavorProfileByNameQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
            public async Task<FlavorProfile?> Handle(GetFlavorProfileByNameQuery request, CancellationToken cancellationToken)
                => await _flavorProfileRepository.GetByNameAsync(request.Name);
        }
}