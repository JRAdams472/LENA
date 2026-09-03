using LENA.Application.Contracts.Persistence;

using MediatR;

namespace LENA.Application.Features.Wine.Bottles.Commands
{
    public record SetBottleFavoriteCommand(int BottleId, bool IsFavorite) : IRequest<Unit>;

    public class SetBottleFavoriteCommandHandler : IRequestHandler<SetBottleFavoriteCommand, Unit>
    {
        private readonly IBottleRepository _bottleRepository;

        public SetBottleFavoriteCommandHandler(IBottleRepository bottleRepository)
        {
            _bottleRepository = bottleRepository;
        }

        public async Task<Unit> Handle(SetBottleFavoriteCommand request, CancellationToken cancellationToken)
        {
            await _bottleRepository.SetFavoriteAsync(request.BottleId, request.IsFavorite, cancellationToken);
            return Unit.Value;
        }
    }
}