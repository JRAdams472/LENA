using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine
{
    // Queries
    public record GetVintageByIdQuery(int VintageId) : IRequest<Vintage?>;
    public record GetVintagesQuery : IRequest<IReadOnlyList<Vintage>>;
    public record GetVintageByYearQuery(int Year) : IRequest<Vintage?>;
    public record GetActiveVintagesQuery : IRequest<IReadOnlyList<Vintage>>;

    // Commands
    public record CreateVintageCommand(Vintage Vintage) : IRequest<Vintage>;
    public record UpdateVintageCommand(Vintage Vintage) : IRequest<Vintage>;
    public record DeleteVintageCommand(int VintageId) : IRequest<Vintage?>;

    // Handlers
    public class GetVintageByIdQueryHandler : IRequestHandler<GetVintageByIdQuery, Vintage?>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintageByIdQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage?> Handle(GetVintageByIdQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.GetByIdAsync(request.VintageId);
    }

    public class GetVintagesQueryHandler : IRequestHandler<GetVintagesQuery, IReadOnlyList<Vintage>>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintagesQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<IReadOnlyList<Vintage>> Handle(GetVintagesQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.ListAllAsync();
    }

    public class GetVintageByYearQueryHandler : IRequestHandler<GetVintageByYearQuery, Vintage?>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintageByYearQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage?> Handle(GetVintageByYearQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.GetByYearAsync(request.Year);
    }

    public class GetActiveVintagesQueryHandler : IRequestHandler<GetActiveVintagesQuery, IReadOnlyList<Vintage>>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetActiveVintagesQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<IReadOnlyList<Vintage>> Handle(GetActiveVintagesQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.GetAllActiveAsync();
    }

    public class CreateVintageCommandHandler : IRequestHandler<CreateVintageCommand, Vintage>
    {
        private readonly IVintageRepository _vintageRepository;
        public CreateVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage> Handle(CreateVintageCommand request, CancellationToken cancellationToken)
            => await _vintageRepository.CreateAsync(request.Vintage);
    }

    public class UpdateVintageCommandHandler : IRequestHandler<UpdateVintageCommand, Vintage>
    {
        private readonly IVintageRepository _vintageRepository;
        public UpdateVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage> Handle(UpdateVintageCommand request, CancellationToken cancellationToken)
            => await _vintageRepository.UpdateAsync(request.Vintage);
    }

    public class DeleteVintageCommandHandler : IRequestHandler<DeleteVintageCommand, Vintage?>
    {
        private readonly IVintageRepository _vintageRepository;
        public DeleteVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage?> Handle(DeleteVintageCommand request, CancellationToken cancellationToken)
        {
            var vintage = await _vintageRepository.GetByIdAsync(request.VintageId);
            if (vintage == null)
                return null;

            return await _vintageRepository.DeleteAsync(vintage);
        }
    }
}
