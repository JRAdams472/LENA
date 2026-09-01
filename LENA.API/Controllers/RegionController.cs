using LENA.Application.Features.Wine.Regions.Commands;
using LENA.Application.Features.Wine.Regions.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class RegionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("regions")]
        public async Task<ActionResult<IReadOnlyList<Region>>> GetRegions()
        {
            var regions = await _mediator.Send(new GetRegionsQuery());
            return Ok(regions);
        }

        [HttpGet("regions/country/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<Region>>> GetRegionsByCountryId(int countryId)
        {
            var regions = await _mediator.Send(new GetRegionsByCountryIdQuery(countryId));
            return Ok(regions);
        }

        [HttpGet("regions/{id}")]
        public async Task<ActionResult<Region?>> GetRegionById(int id)
        {
            var region = await _mediator.Send(new GetRegionByIdQuery(id));
            if (region == null)
                return NotFound();

            return Ok(region);
        }

        [HttpGet("regions/by-name")]
        public async Task<ActionResult<Region?>> GetRegionByNameAndCountryId([FromQuery] string name, [FromQuery] int countryId)
        {
            var region = await _mediator.Send(new GetRegionByNameAndCountryIdQuery(name, countryId));
            if (region == null)
                return NotFound();

            return Ok(region);
        }

        [HttpPost("regions")]
        public async Task<ActionResult<Region>> CreateRegion([FromBody] Region region)
        {
            var created = await _mediator.Send(new CreateRegionCommand(region));
            return CreatedAtAction(nameof(GetRegionById), new { id = created.RegionID }, created);
        }

        [HttpPut("regions/{id}")]
        public async Task<ActionResult<Region>> UpdateRegion(int id, [FromBody] Region region)
        {
            if (id != region.RegionID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateRegionCommand(region));
            return Ok(updated);
        }

        [HttpDelete("regions/{id}")]
        public async Task<ActionResult<Region?>> DeleteRegion(int id)
        {
            var deleted = await _mediator.Send(new DeleteRegionCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }
}
