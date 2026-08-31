using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetBottlesQuery : IRequest<IReadOnlyList<Bottle>>;

    public class GetBottlesQueryHandler : IRequestHandler<GetBottlesQuery, IReadOnlyList<Bottle>>
    {
        private readonly IBottleRepository _bottleRepository;

        public GetBottlesQueryHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<IReadOnlyList<Bottle>> Handle(GetBottlesQuery request, CancellationToken cancellationToken)
        {
            return await _bottleRepository.ListAllAsync();
        }
    }
}
