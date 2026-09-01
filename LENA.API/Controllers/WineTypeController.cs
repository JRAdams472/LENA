using LENA.Application.Features.Wine.Types.Commands;
using LENA.Application.Features.Wine.Types.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class WineTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WineTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("types")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<TypeEntity>>> GetTypes([FromQuery] LENA.Application.Models.PaginationRequest? paging = null)
        {
            var types = await _mediator.Send(new GetTypesQuery(paging));
            return Ok(types);
        }

        [HttpGet("types/{id}")]
        public async Task<ActionResult<TypeEntity?>> GetTypeById(int id)
        {
            var type = await _mediator.Send(new GetTypeByIdQuery(id));
            if (type == null)
                return NotFound();

            return Ok(type);
        }

        [HttpGet("types/name/{name}")]
        public async Task<ActionResult<TypeEntity?>> GetTypeByName(string name)
        {
            var type = await _mediator.Send(new GetTypeByNameQuery(name));
            if (type == null)
                return NotFound();

            return Ok(type);
        }

        [HttpPost("types")]
        public async Task<ActionResult<TypeEntity>> CreateType([FromBody] TypeEntity type)
        {
            var created = await _mediator.Send(new CreateTypeCommand(type));
            return CreatedAtAction(nameof(GetTypeById), new { id = created.TypeID }, created);
        }

        [HttpPut("types/{id}")]
        public async Task<ActionResult<TypeEntity>> UpdateType(int id, [FromBody] TypeEntity type)
        {
            if (id != type.TypeID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateTypeCommand(type));
            return Ok(updated);
        }

        [HttpDelete("types/{id}")]
        public async Task<ActionResult<TypeEntity?>> DeleteType(int id)
        {
            var deleted = await _mediator.Send(new DeleteTypeCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }
}
