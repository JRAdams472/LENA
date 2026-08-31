using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Commands
{
    public record UpdateBottleCommand(Bottle Bottle) : IRequest<Bottle>;

    public class UpdateBottleCommandHandler : IRequestHandler<UpdateBottleCommand, Bottle>
    {
        private readonly IBottleRepository _bottleRepository;

        public UpdateBottleCommandHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<Bottle> Handle(UpdateBottleCommand request, CancellationToken cancellationToken)
        {
            return await _bottleRepository.UpdateAsync(request.Bottle);
        }
    }
}
