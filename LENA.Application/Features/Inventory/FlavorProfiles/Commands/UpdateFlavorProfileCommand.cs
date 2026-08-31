using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Commands
{
    public record UpdateFlavorProfileCommand(FlavorProfile FlavorProfile) : IRequest<FlavorProfile>;

        public class UpdateFlavorProfileCommandHandler : IRequestHandler<UpdateFlavorProfileCommand, FlavorProfile>
        {
            private readonly IFlavorProfileRepository _flavorProfileRepository;
            public UpdateFlavorProfileCommandHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
            public async Task<FlavorProfile> Handle(UpdateFlavorProfileCommand request, CancellationToken cancellationToken)
                => await _flavorProfileRepository.UpdateAsync(request.FlavorProfile, cancellationToken);
        }
}