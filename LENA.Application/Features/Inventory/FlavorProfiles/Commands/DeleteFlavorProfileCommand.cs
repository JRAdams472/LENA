using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using LENA.Application.Exceptions;

namespace LENA.Application.Features.Inventory.FlavorProfiles.Commands
{
    public record DeleteFlavorProfileCommand(int FlavorId) : IRequest<FlavorProfile?>;

    public class DeleteFlavorProfileCommandHandler : IRequestHandler<DeleteFlavorProfileCommand, FlavorProfile?>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public DeleteFlavorProfileCommandHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<FlavorProfile?> Handle(DeleteFlavorProfileCommand request, CancellationToken cancellationToken)
        {
            var flavorProfile = await _flavorProfileRepository.GetByIdAsync(request.FlavorId, cancellationToken) ?? throw new NotFoundException(nameof(FlavorProfile), request.FlavorId);

            return await _flavorProfileRepository.DeleteAsync(flavorProfile, cancellationToken);
        }
    }
}
