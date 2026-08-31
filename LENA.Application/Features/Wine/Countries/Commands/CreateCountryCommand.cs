using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Countries.Commands
{
    public record CreateCountryCommand(Country Country) : IRequest<Country>;

    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Country>
    {
        private readonly ICountryRepository _countryRepository;

        public CreateCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            return await _countryRepository.CreateAsync(request.Country, cancellationToken);
        }
    }
}
