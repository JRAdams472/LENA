using LENA.API.Contracts.Grocery;
using LENA.Application.Features.Grocery.GroceryLists.Commands;
using LENA.Application.Features.Grocery.GroceryLists.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroceryListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroceryListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<GroceryListResponse>>> GetGroceryLists([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetGroceryListsPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<GroceryListResponse>
            {
                Items = paged.Items.Select(GroceryListResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroceryListResponse?>> GetGroceryListById(int id)
        {
            var list = await _mediator.Send(new GetGroceryListByIdQuery(id));
            return Ok(GroceryListResponse.FromEntity(list!));
        }

        [HttpPost("generate")]
        public async Task<ActionResult<GroceryListResponse>> GenerateGroceryList([FromQuery] int? mealPlanId)
        {
            var generated = await _mediator.Send(new GenerateGroceryListCommand(mealPlanId));
            return CreatedAtAction(nameof(GetGroceryListById), new { id = generated.GroceryListID }, GroceryListResponse.FromEntity(generated!));
        }

        [HttpPost("{id}/items")]
        public async Task<ActionResult<GroceryListItemResponse>> AddGroceryListItem(int id, [FromBody] CreateGroceryListItemRequest request)
        {
            var entity = request.ToEntity();
            entity.GroceryListID = id;
            if (string.IsNullOrWhiteSpace(entity.Source))
                entity.Source = "Manual";

            var created = await _mediator.Send(new AddGroceryListItemCommand(entity));
            return Ok(GroceryListItemResponse.FromEntity(created!));
        }

        [HttpPut("items/{groceryListItemId}/checked")]
        public async Task<ActionResult<GroceryListItemResponse>> ToggleGroceryItemChecked(int groceryListItemId)
        {
            var updated = await _mediator.Send(new ToggleGroceryListItemCheckedCommand(groceryListItemId));
            return Ok(GroceryListItemResponse.FromEntity(updated!));
        }

        [HttpDelete("items/{groceryListItemId}")]
        public async Task<IActionResult> DeleteGroceryItem(int groceryListItemId)
        {
            await _mediator.Send(new DeleteGroceryListItemCommand(groceryListItemId));
            return NoContent();
        }
    }
}