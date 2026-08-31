using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Vintages.Commands
{
    public record DeleteVintageCommand(int VintageId) : IRequest<Vintage?>;

    public class DeleteVintageCommandHandler : IRequestHandler<DeleteVintageCommand, Vintage?>
    {
        private readonly IVintageRepository _vintageRepository;
        public DeleteVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
        public async Task<Vintage?> Handle(DeleteVintageCommand request, CancellationToken cancellationToken)
        {
            var vintage = await _vintageRepository.GetByIdAsync(request.VintageId, cancellationToken);
            if (vintage == null)
                return null;

            return await _vintageRepository.DeleteAsync(vintage, cancellationToken);
        }
    }
}
