using LENA.Application.Features.Grocery.GroceryLists.Commands;
using LENA.Application.Features.Grocery.GroceryLists.Queries;
using LENA.Domain.Entity.Grocery;
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
        public async Task<ActionResult<IReadOnlyList<GroceryList>>> GetGroceryLists()
        {
            var lists = await _mediator.Send(new GetGroceryListsQuery());
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroceryList?>> GetGroceryListById(int id)
        {
            var list = await _mediator.Send(new GetGroceryListByIdQuery(id));
            if (list == null)
                return NotFound();

            return Ok(list);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<GroceryList>> GenerateGroceryList([FromQuery] int? mealPlanId)
        {
            var generated = await _mediator.Send(new GenerateGroceryListCommand(mealPlanId));
            return CreatedAtAction(nameof(GetGroceryListById), new { id = generated.GroceryListID }, generated);
        }

        [HttpPost("{id}/items")]
        public async Task<ActionResult<GroceryListItem>> AddGroceryListItem(int id, [FromBody] GroceryListItem item)
        {
            item.GroceryListID = id;
            item.Source = string.IsNullOrWhiteSpace(item.Source) ? "Manual" : item.Source;
            var created = await _mediator.Send(new AddGroceryListItemCommand(item));
            return Ok(created);
        }

        [HttpPut("items/{groceryListItemId}/checked")]
        public async Task<ActionResult<GroceryListItem>> ToggleGroceryItemChecked(int groceryListItemId)
        {
            var updated = await _mediator.Send(new ToggleGroceryListItemCheckedCommand(groceryListItemId));
            return Ok(updated);
        }

        [HttpDelete("items/{groceryListItemId}")]
        public async Task<IActionResult> DeleteGroceryItem(int groceryListItemId)
        {
            await _mediator.Send(new DeleteGroceryListItemCommand(groceryListItemId));
            return NoContent();
        }
    }
}
