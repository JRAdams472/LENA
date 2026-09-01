using LENA.Application.Features.MealPlan.MealPlans.Commands;
using LENA.Application.Features.MealPlan.MealPlans.Queries;
using LENA.Application.Features.MealPlan.MealSlots.Commands;
using LENA.Application.Features.MealPlan.MealSlots.Queries;
using LENA.Application.Features.MealPlan.MealSlotItems.Commands;
using LENA.Application.Features.MealPlan.MealSlotItems.Queries;
using LENA.Application.Features.MealPlan.Queries;
using LENA.Domain.Entity.MealPlan;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;
using MealSlot = LENA.Domain.Entity.MealPlan.MealSlot;
using MealSlotItem = LENA.Domain.Entity.MealPlan.MealSlotItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPlanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MealPlanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("plans")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<MealPlanEntity>>> GetMealPlans([FromQuery] LENA.Application.Models.PaginationRequest? paging = null)
        {
            var plans = await _mediator.Send(new GetMealPlansQuery(paging));
            return Ok(plans);
        }

        [HttpGet("plans/{id}")]
        public async Task<ActionResult<MealPlanEntity?>> GetMealPlanById(int id)
        {
            var plan = await _mediator.Send(new GetMealPlanByIdQuery(id));
            if (plan == null)
                return NotFound();

            return Ok(plan);
        }

        [HttpPost("plans")]
        public async Task<ActionResult<MealPlanEntity>> CreateMealPlan([FromBody] MealPlanEntity mealPlan)
        {
            var created = await _mediator.Send(new CreateMealPlanCommand(mealPlan));
            return CreatedAtAction(nameof(GetMealPlanById), new { id = created.MealPlanID }, created);
        }

        [HttpPut("plans/{id}")]
        public async Task<ActionResult<MealPlanEntity>> UpdateMealPlan(int id, [FromBody] MealPlanEntity mealPlan)
        {
            if (id != mealPlan.MealPlanID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateMealPlanCommand(mealPlan));
            return Ok(updated);
        }

        [HttpDelete("plans/{id}")]
        public async Task<ActionResult<MealPlanEntity?>> DeleteMealPlan(int id)
        {
            var deleted = await _mediator.Send(new DeleteMealPlanCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }

        [HttpGet("plans/{id}/slots")]
        public async Task<ActionResult<IReadOnlyList<MealSlot>>> GetMealSlots(int id)
        {
            var slots = await _mediator.Send(new GetMealSlotsByMealPlanIdQuery(id));
            return Ok(slots);
        }

        [HttpPost("plans/{id}/slots")]
        public async Task<ActionResult<MealSlot>> AddMealSlot(int id, [FromBody] MealSlot mealSlot)
        {
            mealSlot.MealPlanID = id;
            var created = await _mediator.Send(new CreateMealSlotCommand(mealSlot));
            return Ok(created);
        }

        [HttpPut("plans/{id}/slots/{slotId}")]
        public async Task<ActionResult<MealSlot>> UpdateMealSlot(int id, int slotId, [FromBody] MealSlot mealSlot)
        {
            if (slotId != mealSlot.MealSlotID || id != mealSlot.MealPlanID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateMealSlotCommand(mealSlot));
            return Ok(updated);
        }

        [HttpDelete("plans/{id}/slots/{slotId}")]
        public async Task<IActionResult> DeleteMealSlot(int id, int slotId)
        {
            await _mediator.Send(new DeleteMealSlotCommand(slotId));
            return NoContent();
        }

        [HttpGet("slots/{slotId}/items")]
        public async Task<ActionResult<IReadOnlyList<MealSlotItem>>> GetMealSlotItems(int slotId)
        {
            var items = await _mediator.Send(new GetMealSlotItemsBySlotIdQuery(slotId));
            return Ok(items);
        }

        [HttpPost("slots/{slotId}/items")]
        public async Task<ActionResult<MealSlotItem>> AddMealSlotItem(int slotId, [FromBody] MealSlotItem mealSlotItem)
        {
            mealSlotItem.MealSlotID = slotId;
            var created = await _mediator.Send(new CreateMealSlotItemCommand(mealSlotItem));
            return Ok(created);
        }

        [HttpDelete("slots/{slotId}/items/{mealSlotItemId}")]
        public async Task<IActionResult> DeleteMealSlotItem(int slotId, int mealSlotItemId)
        {
            await _mediator.Send(new DeleteMealSlotItemCommand(mealSlotItemId));
            return NoContent();
        }

        [HttpGet("plans/{id}/nutrition")]
        public async Task<ActionResult<MealPlanNutritionDto>> GetMealPlanNutrition(int id)
        {
            var nutrition = await _mediator.Send(new GetMealPlanNutritionQuery(id));
            return Ok(nutrition);
        }
    }
}
