using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using LENA.Application.Exceptions;

namespace LENA.Application.Features.Wine.Vintages.Queries
{
    public record GetVintageByIdQuery(int VintageId) : IRequest<Vintage?>;

    public class GetVintageByIdQueryHandler : IRequestHandler<GetVintageByIdQuery, Vintage?>
    {
        private readonly IVintageRepository _vintageRepository;
        public GetVintageByIdQueryHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage?> Handle(GetVintageByIdQuery request, CancellationToken cancellationToken)
            => await _vintageRepository.GetByIdAsync(request.VintageId, cancellationToken) ?? throw new NotFoundException(nameof(Vintage), request.VintageId);
    }
}
