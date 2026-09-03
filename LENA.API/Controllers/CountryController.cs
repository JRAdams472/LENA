using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Application.Features.Wine.Countries.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetCountries()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("countries/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<Country>>> GetCountriesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var countries = await _mediator.Send(new GetCountriesPagedQuery(pageNumber, pageSize));
            return Ok(countries);
        }

        [HttpGet("countries/active")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetActiveCountries()
        {
            var countries = await _mediator.Send(new GetActiveCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("countries/{id}")]
        public async Task<ActionResult<Country?>> GetCountryById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id));

            return Ok(country);
        }

        [HttpGet("countries/iso/{isoCode}")]
        public async Task<ActionResult<Country?>> GetCountryByISOCode(string isoCode)
        {
            var country = await _mediator.Send(new GetCountryByISOCodeQuery(isoCode));

            return Ok(country);
        }

        [HttpPost("countries")]
        public async Task<ActionResult<Country>> CreateCountry([FromBody] Country country)
        {
            var created = await _mediator.Send(new CreateCountryCommand(country));
            return CreatedAtAction(nameof(GetCountryById), new { id = created.CountryID }, created);
        }

        [HttpPut("countries/{id}")]
        public async Task<ActionResult<Country>> UpdateCountry(int id, [FromBody] Country country)
        {
            if (id != country.CountryID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateCountryCommand(country));
            return Ok(updated);
        }

        [HttpDelete("countries/{id}")]
        public async Task<ActionResult<Country?>> DeleteCountry(int id)
        {
            var deleted = await _mediator.Send(new DeleteCountryCommand(id));

            return Ok(deleted);
        }
    }
}
