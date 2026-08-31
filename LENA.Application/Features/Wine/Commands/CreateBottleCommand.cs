using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Commands
{
    public record CreateBottleCommand(Bottle Bottle) : IRequest<Bottle>;

    public class CreateBottleCommandHandler : IRequestHandler<CreateBottleCommand, Bottle>
    {
        private readonly IBottleRepository _bottleRepository;

        public CreateBottleCommandHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<Bottle> Handle(CreateBottleCommand request, CancellationToken cancellationToken)
        {
            return await _bottleRepository.CreateAsync(request.Bottle);
        }
    }
}
