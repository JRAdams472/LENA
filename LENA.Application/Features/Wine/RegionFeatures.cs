using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine
{
    // Queries
    public record GetRegionByIdQuery(int RegionId) : IRequest<Region?>;
    public record GetRegionsQuery : IRequest<IReadOnlyList<Region>>;
    public record GetRegionsByCountryIdQuery(int CountryId) : IRequest<IReadOnlyList<Region>>;
    public record GetRegionByNameAndCountryIdQuery(string Name, int CountryId) : IRequest<Region?>;

    // Commands
    public record CreateRegionCommand(Region Region) : IRequest<Region>;
    public record UpdateRegionCommand(Region Region) : IRequest<Region>;
    public record DeleteRegionCommand(int RegionId) : IRequest<Region?>;

    // Handlers
    public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, Region?>
    {
        private readonly IRegionRepository _regionRepository;
        public GetRegionByIdQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<Region?> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
            => await _regionRepository.GetByIdAsync(request.RegionId);
    }

    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, IReadOnlyList<Region>>
    {
        private readonly IRegionRepository _regionRepository;
        public GetRegionsQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<IReadOnlyList<Region>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
            => await _regionRepository.ListAllAsync();
    }

    public class GetRegionsByCountryIdQueryHandler : IRequestHandler<GetRegionsByCountryIdQuery, IReadOnlyList<Region>>
    {
        private readonly IRegionRepository _regionRepository;
        public GetRegionsByCountryIdQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<IReadOnlyList<Region>> Handle(GetRegionsByCountryIdQuery request, CancellationToken cancellationToken)
            => await _regionRepository.GetAllByCountryIdAsync(request.CountryId);
    }

    public class GetRegionByNameAndCountryIdQueryHandler : IRequestHandler<GetRegionByNameAndCountryIdQuery, Region?>
    {
        private readonly IRegionRepository _regionRepository;
        public GetRegionByNameAndCountryIdQueryHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<Region?> Handle(GetRegionByNameAndCountryIdQuery request, CancellationToken cancellationToken)
            => await _regionRepository.GetByNameAndCountryIdAsync(request.Name, request.CountryId);
    }

    public class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, Region>
    {
        private readonly IRegionRepository _regionRepository;
        public CreateRegionCommandHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<Region> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
            => await _regionRepository.CreateAsync(request.Region);
    }

    public class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, Region>
    {
        private readonly IRegionRepository _regionRepository;
        public UpdateRegionCommandHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<Region> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
            => await _regionRepository.UpdateAsync(request.Region);
    }

    public class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, Region?>
    {
        private readonly IRegionRepository _regionRepository;
        public DeleteRegionCommandHandler(IRegionRepository regionRepository) => _regionRepository = regionRepository;
        public async Task<Region?> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
        {
            var region = await _regionRepository.GetByIdAsync(request.RegionId);
            if (region == null)
                return null;

            return await _regionRepository.DeleteAsync(region);
        }
    }
}
