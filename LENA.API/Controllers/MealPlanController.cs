using LENA.API.Contracts.MealPlan;
using LENA.Application.Features.MealPlan.MealPlans.Commands;
using LENA.Application.Features.MealPlan.MealPlans.Queries;
using LENA.Application.Features.MealPlan.MealSlotItems.Commands;
using LENA.Application.Features.MealPlan.MealSlotItems.Queries;
using LENA.Application.Features.MealPlan.MealSlots.Commands;
using LENA.Application.Features.MealPlan.MealSlots.Queries;
using LENA.Application.Features.MealPlan.Queries;

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
        [Obsolete("Use GET /api/MealPlan/plans/paged instead.")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<MealPlanResponse>>> GetMealPlans()
        {
            var paged = await _mediator.Send(new GetMealPlansPagedQuery(1, 25));
            return Ok(new LENA.Application.Models.PagedResult<MealPlanResponse>
            {
                Items = paged.Items.Select(MealPlanResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("plans/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<MealPlanResponse>>> GetMealPlansPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetMealPlansPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<MealPlanResponse>
            {
                Items = paged.Items.Select(MealPlanResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("plans/{id}")]
        public async Task<ActionResult<MealPlanResponse?>> GetMealPlanById(int id)
        {
            var plan = await _mediator.Send(new GetMealPlanByIdQuery(id));
            return Ok(MealPlanResponse.FromEntity(plan!));
        }

        [HttpPost("plans")]
        public async Task<ActionResult<MealPlanResponse>> CreateMealPlan([FromBody] CreateMealPlanRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateMealPlanCommand(entity));
            return CreatedAtAction(nameof(GetMealPlanById), new { id = created.MealPlanID }, MealPlanResponse.FromEntity(created!));
        }

        [HttpPut("plans/{id}")]
        public async Task<ActionResult<MealPlanResponse>> UpdateMealPlan(int id, [FromBody] UpdateMealPlanRequest request)
        {
            if (id != request.MealPlanID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateMealPlanCommand(entity));
            return Ok(MealPlanResponse.FromEntity(updated!));
        }

        [HttpDelete("plans/{id}")]
        public async Task<ActionResult<MealPlanResponse?>> DeleteMealPlan(int id)
        {
            var deleted = await _mediator.Send(new DeleteMealPlanCommand(id));
            return Ok(MealPlanResponse.FromEntity(deleted!));
        }

        [HttpGet("plans/{id}/slots")]
        public async Task<ActionResult<IReadOnlyList<MealSlotResponse>>> GetMealSlots(int id)
        {
            var slots = await _mediator.Send(new GetMealSlotsByMealPlanIdQuery(id));
            return Ok(slots.Select(MealSlotResponse.FromEntity).ToList());
        }

        [HttpPost("plans/{id}/slots")]
        public async Task<ActionResult<MealSlotResponse>> AddMealSlot(int id, [FromBody] CreateMealSlotRequest request)
        {
            var entity = request.ToEntity();
            entity.MealPlanID = id;
            var created = await _mediator.Send(new CreateMealSlotCommand(entity));
            return Ok(MealSlotResponse.FromEntity(created!));
        }

        [HttpPut("plans/{id}/slots/{slotId}")]
        public async Task<ActionResult<MealSlotResponse>> UpdateMealSlot(int id, int slotId, [FromBody] UpdateMealSlotRequest request)
        {
            if (slotId != request.MealSlotID)
                return BadRequest();

            var entity = request.ToEntity();
            entity.MealPlanID = id;
            var updated = await _mediator.Send(new UpdateMealSlotCommand(entity));
            return Ok(MealSlotResponse.FromEntity(updated!));
        }

        [HttpDelete("plans/{id}/slots/{slotId}")]
        public async Task<IActionResult> DeleteMealSlot(int id, int slotId)
        {
            await _mediator.Send(new DeleteMealSlotCommand(slotId));
            return NoContent();
        }

        [HttpGet("slots/{slotId}/items")]
        public async Task<ActionResult<IReadOnlyList<MealSlotItemResponse>>> GetMealSlotItems(int slotId)
        {
            var items = await _mediator.Send(new GetMealSlotItemsBySlotIdQuery(slotId));
            return Ok(items.Select(MealSlotItemResponse.FromEntity).ToList());
        }

        [HttpPost("slots/{slotId}/items")]
        public async Task<ActionResult<MealSlotItemResponse>> AddMealSlotItem(int slotId, [FromBody] CreateMealSlotItemRequest request)
        {
            var entity = request.ToEntity();
            entity.MealSlotID = slotId;
            var created = await _mediator.Send(new CreateMealSlotItemCommand(entity));
            return Ok(MealSlotItemResponse.FromEntity(created!));
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