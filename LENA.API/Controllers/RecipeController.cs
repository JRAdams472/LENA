using LENA.API.Contracts.Recipe;
using LENA.Application.Features.Recipe.RecipeItems.Commands;
using LENA.Application.Features.Recipe.RecipeItems.Queries;
using LENA.Application.Features.Recipe.Recipes.Commands;
using LENA.Application.Features.Recipe.Recipes.Queries;
using LENA.Application.Features.Recipe.RecipeSteps.Commands;
using LENA.Application.Features.Recipe.RecipeSteps.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("recipes")]
        [Obsolete("Use GET /api/Recipe/recipes/paged instead.")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<RecipeResponse>>> GetRecipes()
        {
            var paged = await _mediator.Send(new GetRecipesPagedQuery(1, 25, null, false));
            return Ok(new LENA.Application.Models.PagedResult<RecipeResponse>
            {
                Items = paged.Items.Select(RecipeResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("recipes/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<RecipeResponse>>> GetRecipesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25, [FromQuery] string? search = null, [FromQuery] bool isFavorite = false)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetRecipesPagedQuery(pageNumber, pageSize, search, isFavorite));
            return Ok(new LENA.Application.Models.PagedResult<RecipeResponse>
            {
                Items = paged.Items.Select(RecipeResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("recipes/{id}")]
        public async Task<ActionResult<RecipeResponse?>> GetRecipeById(int id)
        {
            var recipe = await _mediator.Send(new GetRecipeByIdQuery(id));
            return Ok(RecipeResponse.FromEntity(recipe!));
        }

        [HttpPost("recipes")]
        public async Task<ActionResult<RecipeResponse>> CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateRecipeCommand(entity));
            return CreatedAtAction(nameof(GetRecipeById), new { id = created.RecipeID }, RecipeResponse.FromEntity(created!));
        }

        [HttpPut("recipes/{id}")]
        public async Task<ActionResult<RecipeResponse>> UpdateRecipe(int id, [FromBody] UpdateRecipeRequest request)
        {
            if (id != request.RecipeID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateRecipeCommand(entity));
            return Ok(RecipeResponse.FromEntity(updated!));
        }

        [HttpDelete("recipes/{id}")]
        public async Task<ActionResult<RecipeResponse?>> DeleteRecipe(int id)
        {
            var deleted = await _mediator.Send(new DeleteRecipeCommand(id));
            return Ok(RecipeResponse.FromEntity(deleted!));
        }

        [HttpPost("recipes/{id}/favorite")]
        public async Task<IActionResult> SetRecipeFavorite(int id, [FromQuery] bool isFavorite)
        {
            await _mediator.Send(new SetRecipeFavoriteCommand(id, isFavorite));
            return NoContent();
        }

        [HttpGet("recipes/{id}/items")]
        public async Task<ActionResult<IReadOnlyList<RecipeItemResponse>>> GetRecipeItems(int id)
        {
            var items = await _mediator.Send(new GetRecipeItemsByRecipeIdQuery(id));
            return Ok(items.Select(RecipeItemResponse.FromEntity).ToList());
        }

        [HttpPost("recipes/{id}/items")]
        public async Task<ActionResult<RecipeItemResponse>> AddRecipeItem(int id, [FromBody] CreateRecipeItemRequest request)
        {
            var recipeItem = request.ToEntity();
            recipeItem.RecipeID = id;

            var created = await _mediator.Send(new AddOrUpdateRecipeItemCommand(recipeItem));
            return Ok(RecipeItemResponse.FromEntity(created!));
        }

        [HttpDelete("recipes/{id}/items/{itemId}")]
        public async Task<IActionResult> RemoveRecipeItem(int id, int itemId)
        {
            await _mediator.Send(new RemoveRecipeItemCommand(id, itemId));
            return NoContent();
        }

        [HttpGet("recipes/{id}/steps")]
        public async Task<ActionResult<IReadOnlyList<RecipeStepResponse>>> GetRecipeSteps(int id)
        {
            var steps = await _mediator.Send(new GetRecipeStepsByRecipeIdQuery(id));
            return Ok(steps.Select(RecipeStepResponse.FromEntity).ToList());
        }

        [HttpPost("recipes/{id}/steps")]
        public async Task<ActionResult<RecipeStepResponse>> AddRecipeStep(int id, [FromBody] CreateRecipeStepRequest request)
        {
            var step = request.ToEntity();
            step.RecipeID = id;

            var created = await _mediator.Send(new AddRecipeStepCommand(step));
            return Ok(RecipeStepResponse.FromEntity(created!));
        }

        [HttpPut("recipes/{id}/steps/{stepId}")]
        public async Task<ActionResult<RecipeStepResponse>> UpdateRecipeStep(int id, int stepId, [FromBody] UpdateRecipeStepRequest request)
        {
            if (stepId != request.RecipeStepID)
                return BadRequest();

            var step = request.ToEntity();
            step.RecipeStepID = stepId;
            step.RecipeID = id;

            var updated = await _mediator.Send(new UpdateRecipeStepCommand(step));
            return Ok(RecipeStepResponse.FromEntity(updated!));
        }

        [HttpDelete("recipes/{id}/steps/{stepId}")]
        public async Task<IActionResult> DeleteRecipeStep(int id, int stepId)
        {
            await _mediator.Send(new DeleteRecipeStepCommand(stepId, id));
            return NoContent();
        }
    }
}