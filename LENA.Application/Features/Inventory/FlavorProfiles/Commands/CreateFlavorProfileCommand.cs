using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

using MediatR;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Commands
{
    public record CreateFlavorProfileCommand(FlavorProfile FlavorProfile) : IRequest<FlavorProfile>;

    public class CreateFlavorProfileCommandHandler : IRequestHandler<CreateFlavorProfileCommand, FlavorProfile>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public CreateFlavorProfileCommandHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<FlavorProfile> Handle(CreateFlavorProfileCommand request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.CreateAsync(request.FlavorProfile, cancellationToken);
    }
}