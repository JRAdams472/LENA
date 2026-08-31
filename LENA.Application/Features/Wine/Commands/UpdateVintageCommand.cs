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
    public record UpdateVintageCommand(Vintage Vintage) : IRequest<Vintage>;

        public class UpdateVintageCommandHandler : IRequestHandler<UpdateVintageCommand, Vintage>
        {
            private readonly IVintageRepository _vintageRepository;
            public UpdateVintageCommandHandler(IVintageRepository vintageRepository) => _vintageRepository = vintageRepository;
            public async Task<Vintage> Handle(UpdateVintageCommand request, CancellationToken cancellationToken)
                => await _vintageRepository.UpdateAsync(request.Vintage);
        }
}