using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;

namespace LENA.Application.Features.Wine.Countries.Commands
{
    public record UpdateCountryCommand(Country Country) : IRequest<Country>;

    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Country>
    {
        private readonly ICountryRepository _countryRepository;

        public UpdateCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            return await _countryRepository.UpdateAsync(request.Country);
        }
    }
}
