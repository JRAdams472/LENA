using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Inventory
{
    // Queries
    public record GetFlavorProfileByIdQuery(int FlavorId) : IRequest<FlavorProfile?>;
    public record GetFlavorProfilesQuery : IRequest<IReadOnlyList<FlavorProfile>>;
    public record GetFlavorProfileByNameQuery(string Name) : IRequest<FlavorProfile?>;
    public record GetActiveFlavorProfilesQuery : IRequest<IReadOnlyList<FlavorProfile>>;

    // Commands
    public record CreateFlavorProfileCommand(FlavorProfile FlavorProfile) : IRequest<FlavorProfile>;
    public record UpdateFlavorProfileCommand(FlavorProfile FlavorProfile) : IRequest<FlavorProfile>;
    public record DeleteFlavorProfileCommand(int FlavorId) : IRequest<FlavorProfile?>;

    // Handlers
    public class GetFlavorProfileByIdQueryHandler : IRequestHandler<GetFlavorProfileByIdQuery, FlavorProfile?>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public GetFlavorProfileByIdQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<FlavorProfile?> Handle(GetFlavorProfileByIdQuery request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.GetByIdAsync(request.FlavorId);
    }

    public class GetFlavorProfilesQueryHandler : IRequestHandler<GetFlavorProfilesQuery, IReadOnlyList<FlavorProfile>>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public GetFlavorProfilesQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<IReadOnlyList<FlavorProfile>> Handle(GetFlavorProfilesQuery request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.ListAllAsync();
    }

    public class GetFlavorProfileByNameQueryHandler : IRequestHandler<GetFlavorProfileByNameQuery, FlavorProfile?>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public GetFlavorProfileByNameQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<FlavorProfile?> Handle(GetFlavorProfileByNameQuery request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.GetByNameAsync(request.Name);
    }

    public class GetActiveFlavorProfilesQueryHandler : IRequestHandler<GetActiveFlavorProfilesQuery, IReadOnlyList<FlavorProfile>>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public GetActiveFlavorProfilesQueryHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<IReadOnlyList<FlavorProfile>> Handle(GetActiveFlavorProfilesQuery request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.GetAllActiveAsync();
    }

    public class CreateFlavorProfileCommandHandler : IRequestHandler<CreateFlavorProfileCommand, FlavorProfile>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public CreateFlavorProfileCommandHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<FlavorProfile> Handle(CreateFlavorProfileCommand request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.CreateAsync(request.FlavorProfile);
    }

    public class UpdateFlavorProfileCommandHandler : IRequestHandler<UpdateFlavorProfileCommand, FlavorProfile>
    {
        private readonly IFlavorProfileRepository _flavorProfileRepository;
        public UpdateFlavorProfileCommandHandler(IFlavorProfileRepository flavorProfileRepository) => _flavorProfileRepository = flavorProfileRepository;
        public async Task<FlavorProfile> Handle(UpdateFlavorProfileCommand request, CancellationToken cancellationToken)
            => await _flavorProfileRepository.UpdateAsync(request.FlavorProfile);
    }

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
