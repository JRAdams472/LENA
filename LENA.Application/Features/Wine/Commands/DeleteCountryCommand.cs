using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LENA.Application.Features.Wine.Commands
{
    public record DeleteCountryCommand(int CountryId) : IRequest<Country?>;

    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Country?>
    {
        private readonly ICountryRepository _countryRepository;

        public DeleteCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country?> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetByIdAsync(request.CountryId);
            if (country == null)
                return null;

            return await _countryRepository.DeleteAsync(country);
        }
    }
}
