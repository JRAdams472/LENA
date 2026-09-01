using LENA.Application.Features.Wine.Vintages.Commands;
using LENA.Application.Features.Wine.Vintages.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<LENA.Application.Models.PagedResult<Vintage>>> GetVintages([FromQuery] LENA.Application.Models.PaginationRequest? paging = null)
        {
            var vintages = await _mediator.Send(new GetVintagesQuery(paging));
            return Ok(vintages);
        }

        [HttpGet("vintages/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<Vintage>>> GetVintagesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            var vintages = await _mediator.Send(new GetVintagesPagedQuery(pageNumber, pageSize));
            return Ok(vintages);
        }

        [HttpGet("vintages/active")]
        public async Task<ActionResult<IReadOnlyList<Vintage>>> GetActiveVintages()
        {
            var vintages = await _mediator.Send(new GetActiveVintagesQuery());
            return Ok(vintages);
        }

        [HttpGet("vintages/{id}")]
        public async Task<ActionResult<Vintage?>> GetVintageById(int id)
        {
            var vintage = await _mediator.Send(new GetVintageByIdQuery(id));
            if (vintage == null)
                return NotFound();

            return Ok(vintage);
        }

        [HttpGet("vintages/year/{year}")]
        public async Task<ActionResult<Vintage?>> GetVintageByYear(int year)
        {
            var vintage = await _mediator.Send(new GetVintageByYearQuery(year));
            if (vintage == null)
                return NotFound();

            return Ok(vintage);
        }

        [HttpPost("vintages")]
        public async Task<ActionResult<Vintage>> CreateVintage([FromBody] Vintage vintage)
        {
            var created = await _mediator.Send(new CreateVintageCommand(vintage));
            return CreatedAtAction(nameof(GetVintageById), new { id = created.VintageID }, created);
        }

        [HttpPut("vintages/{id}")]
        public async Task<ActionResult<Vintage>> UpdateVintage(int id, [FromBody] Vintage vintage)
        {
            if (id != vintage.VintageID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateVintageCommand(vintage));
            return Ok(updated);
        }

        [HttpDelete("vintages/{id}")]
        public async Task<ActionResult<Vintage?>> DeleteVintage(int id)
        {
            var deleted = await _mediator.Send(new DeleteVintageCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }
}
