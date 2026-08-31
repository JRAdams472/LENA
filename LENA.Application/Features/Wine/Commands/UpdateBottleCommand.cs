using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine.Commands
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
