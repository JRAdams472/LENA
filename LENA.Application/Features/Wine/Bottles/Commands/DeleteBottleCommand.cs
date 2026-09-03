using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Wine;

using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Commands
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
            var bottle = await _bottleRepository.GetByIdAsync(request.BottleId, cancellationToken) ?? throw new NotFoundException(nameof(Bottle), request.BottleId);

            return await _bottleRepository.DeleteAsync(bottle, cancellationToken);
        }
    }
}