using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Vintages.Commands;
using LENA.Application.Features.Wine.Vintages.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using LENA.API.Filters;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class VintageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VintageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("vintages")]
        [CacheHeaders(300)]
        public async Task<ActionResult<IReadOnlyList<VintageResponse>>> GetVintages()
        {
            var vintages = await _mediator.Send(new GetVintagesQuery());
            return Ok(vintages.Select(VintageResponse.FromEntity));
        }

        [HttpGet("vintages/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<VintageResponse>>> GetVintagesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetVintagesPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<VintageResponse>
            {
                Items = paged.Items.Select(VintageResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [CacheHeaders(300)]
        [HttpGet("vintages/active")]
        public async Task<ActionResult<IReadOnlyList<VintageResponse>>> GetActiveVintages()
        {
            var vintages = await _mediator.Send(new GetActiveVintagesQuery());
            return Ok(vintages.Select(VintageResponse.FromEntity));
        }

        [HttpGet("vintages/{id}")]
        public async Task<ActionResult<VintageResponse?>> GetVintageById(int id)
        {
            var vintage = await _mediator.Send(new GetVintageByIdQuery(id));
            return Ok(VintageResponse.FromEntity(vintage!));
        }

        [HttpGet("vintages/year/{year}")]
        public async Task<ActionResult<VintageResponse?>> GetVintageByYear(int year)
        {
            var vintage = await _mediator.Send(new GetVintageByYearQuery(year));
            return Ok(VintageResponse.FromEntity(vintage!));
        }

        [HttpPost("vintages")]
        public async Task<ActionResult<VintageResponse>> CreateVintage([FromBody] CreateVintageRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateVintageCommand(entity));
            return CreatedAtAction(nameof(GetVintageById), new { id = created.VintageID }, VintageResponse.FromEntity(created!));
        }

        [HttpPut("vintages/{id}")]
        public async Task<ActionResult<VintageResponse>> UpdateVintage(int id, [FromBody] UpdateVintageRequest request)
        {
            if (id != request.VintageID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateVintageCommand(entity));
            return Ok(VintageResponse.FromEntity(updated!));
        }

        [HttpDelete("vintages/{id}")]
        public async Task<ActionResult<VintageResponse?>> DeleteVintage(int id)
        {
            var deleted = await _mediator.Send(new DeleteVintageCommand(id));
            return Ok(VintageResponse.FromEntity(deleted!));
        }
    }
}