using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Types.Commands;
using LENA.Application.Features.Wine.Types.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

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
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<IReadOnlyList<TypeResponse>>> GetTypes()
        {
            var types = await _mediator.Send(new GetTypesQuery());
            return Ok(types.Select(TypeResponse.FromEntity));
        }

        [HttpGet("types/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<TypeResponse>>> GetTypesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetTypesPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<TypeResponse>
            {
                Items = paged.Items.Select(TypeResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("types/{id}")]
        public async Task<ActionResult<TypeResponse?>> GetTypeById(int id)
        {
            var type = await _mediator.Send(new GetTypeByIdQuery(id));
            return Ok(TypeResponse.FromEntity(type!));
        }

        [HttpGet("types/name/{name}")]
        public async Task<ActionResult<TypeResponse?>> GetTypeByName(string name)
        {
            var type = await _mediator.Send(new GetTypeByNameQuery(name));
            return Ok(TypeResponse.FromEntity(type!));
        }

        [HttpPost("types")]
        public async Task<ActionResult<TypeResponse>> CreateType([FromBody] CreateTypeRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateTypeCommand(entity));
            return CreatedAtAction(nameof(GetTypeById), new { id = created.TypeID }, TypeResponse.FromEntity(created!));
        }

        [HttpPut("types/{id}")]
        public async Task<ActionResult<TypeResponse>> UpdateType(int id, [FromBody] UpdateTypeRequest request)
        {
            if (id != request.TypeID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateTypeCommand(entity));
            return Ok(TypeResponse.FromEntity(updated!));
        }

        [HttpDelete("types/{id}")]
        public async Task<ActionResult<TypeResponse?>> DeleteType(int id)
        {
            var deleted = await _mediator.Send(new DeleteTypeCommand(id));
            return Ok(TypeResponse.FromEntity(deleted!));
        }
    }
}