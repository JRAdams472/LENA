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
    public record GetFlavorProfileByIdQuery(int FlavorId) : IRequest<FlavorProfile?>;

        public class GetFlavorProfileByIdQueryHandler : IRequestHandler<GetFlavorProfileByIdQuery, FlavorProfile?>
        {
            private readonly IFlavorProfileRepository _flavorProfileRepository;
            public GetFlavorProfileByIdQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
            public async Task<FlavorProfile?> Handle(GetFlavorProfileByIdQuery request, CancellationToken cancellationToken)
                => await _flavorProfileRepository.GetByIdAsync(request.FlavorId);
        }
}