using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Application.Features.Wine.Countries.Queries;

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
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<IReadOnlyList<CountryResponse>>> GetCountries()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            return Ok(countries.Select(CountryResponse.FromEntity));
        }

        [HttpGet("countries/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<CountryResponse>>> GetCountriesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetCountriesPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<CountryResponse>
            {
                Items = paged.Items.Select(CountryResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("countries/active")]
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<IReadOnlyList<CountryResponse>>> GetActiveCountries()
        {
            var countries = await _mediator.Send(new GetActiveCountriesQuery());
            return Ok(countries.Select(CountryResponse.FromEntity));
        }

        [HttpGet("countries/{id}")]
        public async Task<ActionResult<CountryResponse?>> GetCountryById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id));
            return Ok(CountryResponse.FromEntity(country!));
        }

        [HttpGet("countries/iso/{isoCode}")]
        public async Task<ActionResult<CountryResponse?>> GetCountryByISOCode(string isoCode)
        {
            var country = await _mediator.Send(new GetCountryByISOCodeQuery(isoCode));
            return Ok(CountryResponse.FromEntity(country!));
        }

        [HttpPost("countries")]
        public async Task<ActionResult<CountryResponse>> CreateCountry([FromBody] CreateCountryRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateCountryCommand(entity));
            return CreatedAtAction(nameof(GetCountryById), new { id = created.CountryID }, CountryResponse.FromEntity(created!));
        }

        [HttpPut("countries/{id}")]
        public async Task<ActionResult<CountryResponse>> UpdateCountry(int id, [FromBody] UpdateCountryRequest request)
        {
            if (id != request.CountryID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateCountryCommand(entity));
            return Ok(CountryResponse.FromEntity(updated!));
        }

        [HttpDelete("countries/{id}")]
        public async Task<ActionResult<CountryResponse?>> DeleteCountry(int id)
        {
            var deleted = await _mediator.Send(new DeleteCountryCommand(id));
            return Ok(CountryResponse.FromEntity(deleted!));
        }
    }
}