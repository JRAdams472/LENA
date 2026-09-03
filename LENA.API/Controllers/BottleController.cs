using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Application.Features.Wine.Bottles.Queries;

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
        [Obsolete("Use GET /api/Wine/bottles/paged instead.")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<BottleResponse>>> GetBottles()
        {
            var paged = await _mediator.Send(new GetBottlesPagedQuery(1, 25));
            return Ok(new LENA.Application.Models.PagedResult<BottleResponse>
            {
                Items = paged.Items.Select(BottleResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("bottles/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<BottleResponse>>> GetBottlesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetBottlesPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<BottleResponse>
            {
                Items = paged.Items.Select(BottleResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("bottles/{id}")]
        public async Task<ActionResult<BottleResponse?>> GetBottleById(int id)
        {
            var bottle = await _mediator.Send(new GetBottleByIdQuery(id));
            return Ok(BottleResponse.FromEntity(bottle!));
        }

        [HttpGet("bottles/country/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<BottleResponse>>> GetBottlesByCountryId(int countryId)
        {
            var bottles = await _mediator.Send(new GetBottlesByCountryIdQuery(countryId));
            return Ok(bottles.Select(BottleResponse.FromEntity));
        }

        [HttpGet("bottles/region/{regionId}")]
        public async Task<ActionResult<IReadOnlyList<BottleResponse>>> GetBottlesByRegionId(int regionId)
        {
            var bottles = await _mediator.Send(new GetBottlesByRegionIdQuery(regionId));
            return Ok(bottles.Select(BottleResponse.FromEntity));
        }

        [HttpGet("bottles/type/{typeId}")]
        public async Task<ActionResult<IReadOnlyList<BottleResponse>>> GetBottlesByTypeId(int typeId)
        {
            var bottles = await _mediator.Send(new GetBottlesByTypeIdQuery(typeId));
            return Ok(bottles.Select(BottleResponse.FromEntity));
        }

        [HttpGet("bottles/vintage/{vintageYear}")]
        public async Task<ActionResult<IReadOnlyList<BottleResponse>>> GetBottlesByVintageYear(int vintageYear)
        {
            var bottles = await _mediator.Send(new GetBottlesByVintageYearQuery(vintageYear));
            return Ok(bottles.Select(BottleResponse.FromEntity));
        }

        [HttpGet("bottles/favorites")]
        public async Task<ActionResult<IReadOnlyList<BottleResponse>>> GetFavoriteBottles()
        {
            var bottles = await _mediator.Send(new GetFavoriteBottlesQuery());
            return Ok(bottles.Select(BottleResponse.FromEntity));
        }

        [HttpGet("bottles/search")]
        public async Task<ActionResult<IReadOnlyList<BottleResponse>>> SearchBottles([FromQuery] string searchTerm)
        {
            var bottles = await _mediator.Send(new SearchBottlesQuery(searchTerm));
            return Ok(bottles.Select(BottleResponse.FromEntity));
        }

        [HttpGet("bottles/count")]
        public async Task<ActionResult<int>> GetTotalBottleCount()
        {
            var count = await _mediator.Send(new GetTotalBottleCountQuery());
            return Ok(count);
        }

        [HttpPost("bottles")]
        public async Task<ActionResult<BottleResponse>> CreateBottle([FromBody] CreateBottleRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateBottleCommand(entity));
            return CreatedAtAction(nameof(GetBottleById), new { id = created.BottleID }, BottleResponse.FromEntity(created!));
        }

        [HttpPut("bottles/{id}")]
        public async Task<ActionResult<BottleResponse>> UpdateBottle(int id, [FromBody] UpdateBottleRequest request)
        {
            if (id != request.BottleID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateBottleCommand(entity));
            return Ok(BottleResponse.FromEntity(updated!));
        }

        [HttpPost("bottles/{id}/favorite")]
        public async Task<IActionResult> SetBottleFavorite(int id, [FromQuery] bool isFavorite)
        {
            await _mediator.Send(new SetBottleFavoriteCommand(id, isFavorite));
            return NoContent();
        }

        [HttpDelete("bottles/{id}")]
        public async Task<ActionResult<BottleResponse?>> DeleteBottle(int id)
        {
            var deleted = await _mediator.Send(new DeleteBottleCommand(id));
            return Ok(BottleResponse.FromEntity(deleted!));
        }
    }
}