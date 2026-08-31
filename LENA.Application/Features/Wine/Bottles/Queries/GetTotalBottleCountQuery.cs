using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetTotalBottleCountQuery : IRequest<int>;

    public class GetTotalBottleCountQueryHandler : IRequestHandler<GetTotalBottleCountQuery, int>
    {
        private readonly IBottleRepository _bottleRepository;
        public GetTotalBottleCountQueryHandler(IBottleRepository bottleRepository) => _bottleRepository = bottleRepository;
        public async Task<int> Handle(GetTotalBottleCountQuery request, CancellationToken cancellationToken)
            => await _bottleRepository.GetTotalBottleCountAsync(cancellationToken);
    }
}
