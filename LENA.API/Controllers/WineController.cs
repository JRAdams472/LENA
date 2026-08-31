using LENA.Application.Features.Wine.Commands;
using LENA.Application.Features.Wine.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Bottle endpoints
        [HttpGet("bottles")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottles()
        {
            var bottles = await _mediator.Send(new GetBottlesQuery());
            return Ok(bottles);
        }

        [HttpGet("bottles/{id}")]
        public async Task<ActionResult<Bottle?>> GetBottleById(int id)
        {
            var bottle = await _mediator.Send(new GetBottleByIdQuery(id));
            if (bottle == null)
                return NotFound();

            return Ok(bottle);
        }

        [HttpPost("bottles")]
        public async Task<ActionResult<Bottle>> CreateBottle([FromBody] Bottle bottle)
        {
            var created = await _mediator.Send(new CreateBottleCommand(bottle));
            return CreatedAtAction(nameof(GetBottleById), new { id = created.BottleID }, created);
        }

        [HttpPut("bottles/{id}")]
        public async Task<ActionResult<Bottle>> UpdateBottle(int id, [FromBody] Bottle bottle)
        {
            if (id != bottle.BottleID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateBottleCommand(bottle));
            return Ok(updated);
        }

        [HttpDelete("bottles/{id}")]
        public async Task<ActionResult<Bottle?>> DeleteBottle(int id)
        {
            var deleted = await _mediator.Send(new DeleteBottleCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }

        // Country endpoints
        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetCountries()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("countries/{id}")]
        public async Task<ActionResult<Country?>> GetCountryById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id));
            if (country == null)
                return NotFound();

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
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }
}
