using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using MediatR;
using LENA.Application.Exceptions;

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
            var country = await _countryRepository.GetByIdAsync(request.CountryId, cancellationToken) ?? throw new NotFoundException(nameof(Country), request.CountryId);

            return await _countryRepository.DeleteAsync(country, cancellationToken);
        }
    }
}
