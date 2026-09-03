using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using LENA.Application.Exceptions;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Queries
{
    public record GetFlavorProfileByIdQuery(int FlavorId) : IRequest<FlavorProfile?>;

    public class GetFlavorProfileByIdQueryHandler : IRequestHandler<GetFlavorProfileByIdQuery, FlavorProfile?>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public GetFlavorProfileByIdQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<FlavorProfile?> Handle(GetFlavorProfileByIdQuery request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.GetByIdAsync(request.FlavorId, cancellationToken) ?? throw new NotFoundException(nameof(FlavorProfile), request.FlavorId);
    }
}
