using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Commands
{
    public record DeleteBottleCommand(int BottleId) : IRequest<Bottle?>;

    public class DeleteBottleCommandHandler : IRequestHandler<DeleteBottleCommand, Bottle?>
    {
        private readonly IBottleRepository _bottleRepository;

        public DeleteBottleCommandHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<Bottle?> Handle(DeleteBottleCommand request, CancellationToken cancellationToken)
        {
            var bottle = await _bottleRepository.GetByIdAsync(request.BottleId);
            if (bottle == null)
                return null;

            return await _bottleRepository.DeleteAsync(bottle);
        }
    }
}
