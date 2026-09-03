using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Regions.Commands;
using LENA.Application.Features.Wine.Regions.Queries;

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
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<IReadOnlyList<RegionResponse>>> GetRegions()
        {
            var regions = await _mediator.Send(new GetRegionsQuery());
            return Ok(regions.Select(RegionResponse.FromEntity));
        }

        [HttpGet("regions/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<RegionResponse>>> GetRegionsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetRegionsPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<RegionResponse>
            {
                Items = paged.Items.Select(RegionResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("regions/country/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<RegionResponse>>> GetRegionsByCountryId(int countryId)
        {
            var regions = await _mediator.Send(new GetRegionsByCountryIdQuery(countryId));
            return Ok(regions.Select(RegionResponse.FromEntity));
        }

        [HttpGet("regions/{id}")]
        public async Task<ActionResult<RegionResponse?>> GetRegionById(int id)
        {
            var region = await _mediator.Send(new GetRegionByIdQuery(id));
            return Ok(RegionResponse.FromEntity(region!));
        }

        [HttpGet("regions/by-name")]
        public async Task<ActionResult<RegionResponse?>> GetRegionByNameAndCountryId([FromQuery] string name, [FromQuery] int countryId)
        {
            var region = await _mediator.Send(new GetRegionByNameAndCountryIdQuery(name, countryId));
            return Ok(RegionResponse.FromEntity(region!));
        }

        [HttpPost("regions")]
        public async Task<ActionResult<RegionResponse>> CreateRegion([FromBody] CreateRegionRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateRegionCommand(entity));
            return CreatedAtAction(nameof(GetRegionById), new { id = created.RegionID }, RegionResponse.FromEntity(created!));
        }

        [HttpPut("regions/{id}")]
        public async Task<ActionResult<RegionResponse>> UpdateRegion(int id, [FromBody] UpdateRegionRequest request)
        {
            if (id != request.RegionID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateRegionCommand(entity));
            return Ok(RegionResponse.FromEntity(updated!));
        }

        [HttpDelete("regions/{id}")]
        public async Task<ActionResult<RegionResponse?>> DeleteRegion(int id)
        {
            var deleted = await _mediator.Send(new DeleteRegionCommand(id));
            return Ok(RegionResponse.FromEntity(deleted!));
        }
    }
}