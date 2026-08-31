using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace LENA.Application.Features.Wine.Commands
{
    public record UpdateRegionCommand(Region Region) : IRequest<Region>;

        public class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, Region>
        {
            private readonly IRegionRepository _regionRepository;
            public UpdateRegionCommandHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
            public async Task<Region> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
                => await _regionRepository.UpdateAsync(request.Region);
        }
}