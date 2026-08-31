using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Commands
{
    public record DeleteFlavorProfileCommand(int FlavorId) : IRequest<FlavorProfile?>;

        public class DeleteFlavorProfileCommandHandler : IRequestHandler<DeleteFlavorProfileCommand, FlavorProfile?>
        {
            private readonly IFlavorProfileRepository _flavorProfileRepository;
            public DeleteFlavorProfileCommandHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
            public async Task<FlavorProfile?> Handle(DeleteFlavorProfileCommand request, CancellationToken cancellationToken)
            {
                var flavorProfile = await _flavorProfileRepository.GetByIdAsync(request.FlavorId);
                if (flavorProfile == null)
                    return null;
    
                return await _flavorProfileRepository.DeleteAsync(flavorProfile);
            }
        }
}