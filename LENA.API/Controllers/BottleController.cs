using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Application.Features.Wine.Bottles.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class BottleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BottleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("bottles")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottles()
        {
            var bottles = await _mediator.Send(new GetBottlesQuery());
            return Ok(bottles);
        }

        [HttpGet("bottles/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<Bottle>>> GetBottlesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var bottles = await _mediator.Send(new GetBottlesPagedQuery(pageNumber, pageSize));
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

        [HttpGet("bottles/country/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByCountryId(int countryId)
        {
            var bottles = await _mediator.Send(new GetBottlesByCountryIdQuery(countryId));
            return Ok(bottles);
        }

        [HttpGet("bottles/region/{regionId}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByRegionId(int regionId)
        {
            var bottles = await _mediator.Send(new GetBottlesByRegionIdQuery(regionId));
            return Ok(bottles);
        }

        [HttpGet("bottles/type/{typeId}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByTypeId(int typeId)
        {
            var bottles = await _mediator.Send(new GetBottlesByTypeIdQuery(typeId));
            return Ok(bottles);
        }

        [HttpGet("bottles/vintage/{vintageYear}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByVintageYear(int vintageYear)
        {
            var bottles = await _mediator.Send(new GetBottlesByVintageYearQuery(vintageYear));
            return Ok(bottles);
        }

        [HttpGet("bottles/favorites")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetFavoriteBottles()
        {
            var bottles = await _mediator.Send(new GetFavoriteBottlesQuery());
            return Ok(bottles);
        }

        [HttpGet("bottles/search")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> SearchBottles([FromQuery] string searchTerm)
        {
            var bottles = await _mediator.Send(new SearchBottlesQuery(searchTerm));
            return Ok(bottles);
        }

        [HttpGet("bottles/count")]
        public async Task<ActionResult<int>> GetTotalBottleCount()
        {
            var count = await _mediator.Send(new GetTotalBottleCountQuery());
            return Ok(count);
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

        [HttpPost("bottles/{id}/favorite")]
        public async Task<IActionResult> SetBottleFavorite(int id, [FromQuery] bool isFavorite)
        {
            await _mediator.Send(new SetBottleFavoriteCommand(id, isFavorite));
            return NoContent();
        }

        [HttpDelete("bottles/{id}")]
        public async Task<ActionResult<Bottle?>> DeleteBottle(int id)
        {
            var deleted = await _mediator.Send(new DeleteBottleCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }
}
