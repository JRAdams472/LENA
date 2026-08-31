using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Queries
{
    public record GetBottleByIdQuery(int BottleId) : IRequest<Bottle?>;

    public class GetBottleByIdQueryHandler : IRequestHandler<GetBottleByIdQuery, Bottle?>
    {
        private readonly IBottleRepository _bottleRepository;

        public GetBottleByIdQueryHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<Bottle?> Handle(GetBottleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _bottleRepository.GetByIdAsync(request.BottleId);
        }
    }
}
