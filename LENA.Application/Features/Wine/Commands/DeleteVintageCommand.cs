using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace LENA.Application.Features.Wine.Commands
{
    public record DeleteVintageCommand(int VintageId) : IRequest<Vintage?>;

        public class DeleteVintageCommandHandler : IRequestHandler<DeleteVintageCommand, Vintage?>
        {
            private readonly IVintageRepository _vintageRepository;
            public DeleteVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
            public async Task<Vintage?> Handle(DeleteVintageCommand request, CancellationToken cancellationToken)
            {
                var vintage = await _vintageRepository.GetByIdAsync(request.VintageId);
                if (vintage == null)
                    return null;
    
                return await _vintageRepository.DeleteAsync(vintage);
            }
        }
}