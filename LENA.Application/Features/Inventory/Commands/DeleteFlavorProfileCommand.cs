using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LENA.Application.Features.Inventory.Commands
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