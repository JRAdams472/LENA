using LENA.Application.Features.Recipe.RecipeItems.Commands;
using LENA.Application.Features.Recipe.RecipeItems.Queries;
using LENA.Application.Features.Recipe.Recipes.Commands;
using LENA.Application.Features.Recipe.Recipes.Queries;
using LENA.Application.Features.Recipe.RecipeSteps.Commands;
using LENA.Application.Features.Recipe.RecipeSteps.Queries;
using LENA.Domain.Entity.Recipe;
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
        public async Task<ActionResult<IReadOnlyList<Recipe>>> GetRecipes()
        {
            var recipes = await _mediator.Send(new GetRecipesQuery());
            return Ok(recipes);
        }

        [HttpGet("recipes/{id}")]
        public async Task<ActionResult<Recipe?>> GetRecipeById(int id)
        {
            var recipe = await _mediator.Send(new GetRecipeByIdQuery(id));
            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

        [HttpPost("recipes")]
        public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipe)
        {
            var created = await _mediator.Send(new CreateRecipeCommand(recipe));
            return CreatedAtAction(nameof(GetRecipeById), new { id = created.RecipeID }, created);
        }

        [HttpPut("recipes/{id}")]
        public async Task<ActionResult<Recipe>> UpdateRecipe(int id, [FromBody] Recipe recipe)
        {
            if (id != recipe.RecipeID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateRecipeCommand(recipe));
            return Ok(updated);
        }

        [HttpDelete("recipes/{id}")]
        public async Task<ActionResult<Recipe?>> DeleteRecipe(int id)
        {
            var deleted = await _mediator.Send(new DeleteRecipeCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }

        [HttpGet("recipes/{id}/items")]
        public async Task<ActionResult<IReadOnlyList<RecipeItem>>> GetRecipeItems(int id)
        {
            var items = await _mediator.Send(new GetRecipeItemsByRecipeIdQuery(id));
            return Ok(items);
        }

        [HttpPost("recipes/{id}/items")]
        public async Task<ActionResult<RecipeItem>> AddRecipeItem(int id, [FromBody] RecipeItemRequest request)
        {
            var recipeItem = new RecipeItem
            {
                RecipeID = id,
                ItemID = request.ItemId,
                Quantity = request.Portion,
                UnitOfMeasure = request.Unit,
                IsOptional = request.IsOptional
            };

            var created = await _mediator.Send(new AddOrUpdateRecipeItemCommand(recipeItem));
            return Ok(created);
        }

        [HttpDelete("recipes/{id}/items/{itemId}")]
        public async Task<IActionResult> RemoveRecipeItem(int id, int itemId)
        {
            await _mediator.Send(new RemoveRecipeItemCommand(id, itemId));
            return NoContent();
        }

        [HttpGet("recipes/{id}/steps")]
        public async Task<ActionResult<IReadOnlyList<RecipeStep>>> GetRecipeSteps(int id)
        {
            var steps = await _mediator.Send(new GetRecipeStepsByRecipeIdQuery(id));
            return Ok(steps);
        }

        [HttpPost("recipes/{id}/steps")]
        public async Task<ActionResult<RecipeStep>> AddRecipeStep(int id, [FromBody] RecipeStepRequest request)
        {
            var step = new RecipeStep
            {
                RecipeID = id,
                StepNumber = request.StepNumber,
                Instruction = request.Instruction
            };

            var created = await _mediator.Send(new AddRecipeStepCommand(step));
            return Ok(created);
        }

        [HttpPut("recipes/{id}/steps/{stepId}")]
        public async Task<ActionResult<RecipeStep>> UpdateRecipeStep(int id, int stepId, [FromBody] RecipeStepRequest request)
        {
            var step = new RecipeStep
            {
                RecipeStepID = stepId,
                RecipeID = id,
                StepNumber = request.StepNumber,
                Instruction = request.Instruction
            };

            var updated = await _mediator.Send(new UpdateRecipeStepCommand(step));
            return Ok(updated);
        }

        [HttpDelete("recipes/{id}/steps/{stepId}")]
        public async Task<IActionResult> DeleteRecipeStep(int id, int stepId)
        {
            await _mediator.Send(new DeleteRecipeStepCommand(stepId, id));
            return NoContent();
        }
    }

    public record RecipeItemRequest(int ItemId, decimal Portion, string? Unit, bool IsOptional);
    public record RecipeStepRequest(int StepNumber, string Instruction);
}
