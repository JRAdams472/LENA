using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Countries.Commands
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
            var country = await _countryRepository.GetByIdAsync(request.CountryId, cancellationToken);
            if (country == null)
                return null;

            return await _countryRepository.DeleteAsync(country, cancellationToken);
        }
    }
}
