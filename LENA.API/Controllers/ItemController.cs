using LENA.Application.Features.Inventory.FlavorProfiles.Commands;
using LENA.Application.Features.Inventory.FlavorProfiles.Queries;
using LENA.Application.Features.Inventory.FoodFlavors.Commands;
using LENA.Application.Features.Inventory.FoodFlavors.Queries;
using LENA.Application.Features.Inventory.FoodNutrients.Commands;
using LENA.Application.Features.Inventory.FoodNutrients.Queries;
using LENA.Application.Features.Inventory.Items.Commands;
using LENA.Application.Features.Inventory.Items.Queries;
using LENA.Application.Features.Inventory.NutrientTypes.Commands;
using LENA.Application.Features.Inventory.NutrientTypes.Queries;
using LENA.Domain.Entity.Inventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Item endpoints
        [HttpGet("items")]
        public async Task<ActionResult<IReadOnlyList<Item>>> GetItems()
        {
            var items = await _mediator.Send(new GetItemsQuery());
            return Ok(items);
        }

        [HttpGet("items/search")]
        public async Task<ActionResult<IReadOnlyList<Item>>> SearchItems([FromQuery] string? search = null, [FromQuery] string? brand = null, [FromQuery] int limit = 50)
        {
            if (string.IsNullOrWhiteSpace(brand) && (string.IsNullOrWhiteSpace(search) || search!.Length < 2))
                return Ok(Array.Empty<Item>());

            var items = await _mediator.Send(new SearchItemsQuery(search ?? string.Empty, brand, limit));
            return Ok(items);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands([FromQuery] string? search = null)
        {
            var brands = await _mediator.Send(new GetItemBrandsQuery(search));
            return Ok(brands);
        }

        [HttpGet("items/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<Item>>> GetItemsPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string? search = null,
            [FromQuery] string? brand = null,
            [FromQuery] bool inStock = false,
            [FromQuery] bool isFavorite = false)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var items = await _mediator.Send(new GetItemsPagedQuery(pageNumber, pageSize, search, brand, inStock, isFavorite));
            return Ok(items);
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<Item?>> GetItemById(int id)
        {
            var item = await _mediator.Send(new GetItemByIdQuery(id));

            return Ok(item);
        }

        [HttpGet("items/name/{name}")]
        public async Task<ActionResult<Item?>> GetItemByName(string name)
        {
            var item = await _mediator.Send(new GetItemByNameQuery(name));

            return Ok(item);
        }

        [HttpPost("items")]
        public async Task<ActionResult<Item>> CreateItem([FromBody] Item item)
        {
            var created = await _mediator.Send(new CreateItemCommand(item));
            return CreatedAtAction(nameof(GetItemById), new { id = created.ItemID }, created);
        }

        [HttpPut("items/{id}")]
        public async Task<ActionResult<Item>> UpdateItem(int id, [FromBody] Item item)
        {
            if (id != item.ItemID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateItemCommand(item));
            return Ok(updated);
        }

        [HttpDelete("items/{id}")]
        public async Task<ActionResult<Item?>> DeleteItem(int id)
        {
            var deleted = await _mediator.Send(new DeleteItemCommand(id));

            return Ok(deleted);
        }

        [HttpPost("items/{id}/category/{categoryId}")]
        public async Task<IActionResult> ChangeItemCategory(int id, int categoryId)
        {
            await _mediator.Send(new ChangeItemCategoryCommand(id, categoryId));
            return NoContent();
        }

        [HttpPost("items/{id}/upc12")]
        public async Task<IActionResult> AddOrUpdateUPC12(int id, [FromBody] string upc12)
        {
            await _mediator.Send(new AddOrUpdateItemUPC12Command(id, upc12));
            return NoContent();
        }

        [HttpPost("items/{id}/upc14")]
        public async Task<IActionResult> AddOrUpdateUPC14(int id, [FromBody] string upc14)
        {
            await _mediator.Send(new AddOrUpdateItemUPC14Command(id, upc14));
            return NoContent();
        }

        [HttpPost("items/{id}/quantity")]
        public async Task<IActionResult> AdjustItemQuantity(int id, [FromQuery] decimal quantity, [FromQuery] DateTime? purchaseDate)
        {
            await _mediator.Send(new AdjustItemQuantityCommand(id, quantity, purchaseDate));
            return NoContent();
        }

        [HttpPost("items/{id}/favorite")]
        public async Task<IActionResult> SetItemFavorite(int id, [FromQuery] bool isFavorite)
        {
            await _mediator.Send(new SetItemFavoriteCommand(id, isFavorite));
            return NoContent();
        }

        // FoodFlavor endpoints
        [HttpGet("foodflavors")]
        public async Task<ActionResult<IReadOnlyList<FoodFlavor>>> GetFoodFlavors()
        {
            var foodFlavors = await _mediator.Send(new GetFoodFlavorsQuery());
            return Ok(foodFlavors);
        }

        [HttpGet("foodflavors/{id}")]
        public async Task<ActionResult<FoodFlavor?>> GetFoodFlavorById(int id)
        {
            var foodFlavor = await _mediator.Send(new GetFoodFlavorByIdQuery(id));

            return Ok(foodFlavor);
        }

        [HttpGet("foodflavors/food/{foodId}")]
        public async Task<ActionResult<IEnumerable<FoodFlavor>>> GetFoodFlavorsByFoodId(int foodId)
        {
            var foodFlavors = await _mediator.Send(new GetFoodFlavorsByFoodIdQuery(foodId));
            return Ok(foodFlavors);
        }

        [HttpGet("foodflavors/flavor/{flavorId}")]
        public async Task<ActionResult<IEnumerable<FoodFlavor>>> GetFoodFlavorsByFlavorId(int flavorId)
        {
            var foodFlavors = await _mediator.Send(new GetFoodFlavorsByFlavorIdQuery(flavorId));
            return Ok(foodFlavors);
        }

        [HttpGet("foodflavors/food/{foodId}/flavor/{flavorId}")]
        public async Task<ActionResult<FoodFlavor?>> GetFoodFlavorByFoodAndFlavorId(int foodId, int flavorId)
        {
            var foodFlavor = await _mediator.Send(new GetFoodFlavorByFoodAndFlavorIdQuery(foodId, flavorId));

            return Ok(foodFlavor);
        }

        [HttpPost("foodflavors")]
        public async Task<ActionResult<FoodFlavor>> CreateFoodFlavor([FromBody] FoodFlavor foodFlavor)
        {
            var created = await _mediator.Send(new CreateFoodFlavorCommand(foodFlavor));
            return CreatedAtAction(nameof(GetFoodFlavorByFoodAndFlavorId), new { foodId = created.FoodId, flavorId = created.FlavorId }, created);
        }

        [HttpPut("foodflavors/food/{foodId}/flavor/{flavorId}")]
        public async Task<ActionResult<FoodFlavor>> UpdateFoodFlavor(int foodId, int flavorId, [FromBody] FoodFlavor foodFlavor)
        {
            if (foodId != foodFlavor.FoodId || flavorId != foodFlavor.FlavorId)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateFoodFlavorCommand(foodFlavor));
            return Ok(updated);
        }

        [HttpDelete("foodflavors/food/{foodId}/flavor/{flavorId}")]
        public async Task<ActionResult<FoodFlavor?>> DeleteFoodFlavor(int foodId, int flavorId)
        {
            var deleted = await _mediator.Send(new DeleteFoodFlavorCommand(foodId, flavorId));

            return Ok(deleted);
        }

        // FoodNutrient endpoints
        [HttpGet("foodnutrients")]
        public async Task<ActionResult<IReadOnlyList<FoodNutrient>>> GetFoodNutrients()
        {
            var foodNutrients = await _mediator.Send(new GetFoodNutrientsQuery());
            return Ok(foodNutrients);
        }

        [HttpGet("foodnutrients/{id}")]
        public async Task<ActionResult<FoodNutrient?>> GetFoodNutrientById(int id)
        {
            var foodNutrient = await _mediator.Send(new GetFoodNutrientByIdQuery(id));

            return Ok(foodNutrient);
        }

        [HttpGet("foodnutrients/food/{foodId}")]
        public async Task<ActionResult<IEnumerable<FoodNutrient>>> GetFoodNutrientsByFoodId(int foodId)
        {
            var foodNutrients = await _mediator.Send(new GetFoodNutrientsByFoodIdQuery(foodId));
            return Ok(foodNutrients);
        }

        [HttpGet("foodnutrients/nutrient/{nutrientId}")]
        public async Task<ActionResult<IEnumerable<FoodNutrient>>> GetFoodNutrientsByNutrientId(int nutrientId)
        {
            var foodNutrients = await _mediator.Send(new GetFoodNutrientsByNutrientIdQuery(nutrientId));
            return Ok(foodNutrients);
        }

        [HttpGet("foodnutrients/food/{foodId}/nutrient/{nutrientId}")]
        public async Task<ActionResult<FoodNutrient?>> GetFoodNutrientByFoodAndNutrientId(int foodId, int nutrientId)
        {
            var foodNutrient = await _mediator.Send(new GetFoodNutrientByFoodAndNutrientIdQuery(foodId, nutrientId));

            return Ok(foodNutrient);
        }

        [HttpPost("foodnutrients")]
        public async Task<ActionResult<FoodNutrient>> CreateFoodNutrient([FromBody] FoodNutrient foodNutrient)
        {
            var created = await _mediator.Send(new CreateFoodNutrientCommand(foodNutrient));
            return CreatedAtAction(nameof(GetFoodNutrientByFoodAndNutrientId), new { foodId = created.FoodId, nutrientId = created.NutrientId }, created);
        }

        [HttpPut("foodnutrients/food/{foodId}/nutrient/{nutrientId}")]
        public async Task<ActionResult<FoodNutrient>> UpdateFoodNutrient(int foodId, int nutrientId, [FromBody] FoodNutrient foodNutrient)
        {
            if (foodId != foodNutrient.FoodId || nutrientId != foodNutrient.NutrientId)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateFoodNutrientCommand(foodNutrient));
            return Ok(updated);
        }

        [HttpDelete("foodnutrients/food/{foodId}/nutrient/{nutrientId}")]
        public async Task<ActionResult<FoodNutrient?>> DeleteFoodNutrient(int foodId, int nutrientId)
        {
            var deleted = await _mediator.Send(new DeleteFoodNutrientCommand(foodId, nutrientId));

            return Ok(deleted);
        }

        // NutrientType endpoints
        [HttpGet("nutrienttypes")]
        public async Task<ActionResult<IReadOnlyList<NutrientType>>> GetNutrientTypes()
        {
            var nutrientTypes = await _mediator.Send(new GetNutrientTypesQuery());
            return Ok(nutrientTypes);
        }

        [HttpGet("nutrienttypes/{id}")]
        public async Task<ActionResult<NutrientType?>> GetNutrientTypeById(int id)
        {
            var nutrientType = await _mediator.Send(new GetNutrientTypeByIdQuery(id));

            return Ok(nutrientType);
        }

        [HttpGet("nutrienttypes/name/{name}")]
        public async Task<ActionResult<NutrientType?>> GetNutrientTypeByName(string name)
        {
            var nutrientType = await _mediator.Send(new GetNutrientTypeByNameQuery(name));

            return Ok(nutrientType);
        }

        [HttpPost("nutrienttypes")]
        public async Task<ActionResult<NutrientType>> CreateNutrientType([FromBody] NutrientType nutrientType)
        {
            var created = await _mediator.Send(new CreateNutrientTypeCommand(nutrientType));
            return CreatedAtAction(nameof(GetNutrientTypeById), new { id = created.NutrientId }, created);
        }

        [HttpPut("nutrienttypes/{id}")]
        public async Task<ActionResult<NutrientType>> UpdateNutrientType(int id, [FromBody] NutrientType nutrientType)
        {
            if (id != nutrientType.NutrientId)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateNutrientTypeCommand(nutrientType));
            return Ok(updated);
        }

        [HttpDelete("nutrienttypes/{id}")]
        public async Task<ActionResult<NutrientType?>> DeleteNutrientType(int id)
        {
            var deleted = await _mediator.Send(new DeleteNutrientTypeCommand(id));

            return Ok(deleted);
        }

        // FlavorProfile endpoints
        [HttpGet("flavorprofiles")]
        public async Task<ActionResult<IReadOnlyList<FlavorProfile>>> GetFlavorProfiles()
        {
            var flavorProfiles = await _mediator.Send(new GetFlavorProfilesQuery());
            return Ok(flavorProfiles);
        }

        [HttpGet("flavorprofiles/active")]
        public async Task<ActionResult<IReadOnlyList<FlavorProfile>>> GetActiveFlavorProfiles()
        {
            var flavorProfiles = await _mediator.Send(new GetActiveFlavorProfilesQuery());
            return Ok(flavorProfiles);
        }

        [HttpGet("flavorprofiles/{id}")]
        public async Task<ActionResult<FlavorProfile?>> GetFlavorProfileById(int id)
        {
            var flavorProfile = await _mediator.Send(new GetFlavorProfileByIdQuery(id));

            return Ok(flavorProfile);
        }

        [HttpGet("flavorprofiles/name/{name}")]
        public async Task<ActionResult<FlavorProfile?>> GetFlavorProfileByName(string name)
        {
            var flavorProfile = await _mediator.Send(new GetFlavorProfileByNameQuery(name));

            return Ok(flavorProfile);
        }

        [HttpPost("flavorprofiles")]
        public async Task<ActionResult<FlavorProfile>> CreateFlavorProfile([FromBody] FlavorProfile flavorProfile)
        {
            var created = await _mediator.Send(new CreateFlavorProfileCommand(flavorProfile));
            return CreatedAtAction(nameof(GetFlavorProfileById), new { id = created.FlavorId }, created);
        }

        [HttpPut("flavorprofiles/{id}")]
        public async Task<ActionResult<FlavorProfile>> UpdateFlavorProfile(int id, [FromBody] FlavorProfile flavorProfile)
        {
            if (id != flavorProfile.FlavorId)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateFlavorProfileCommand(flavorProfile));
            return Ok(updated);
        }

        [HttpDelete("flavorprofiles/{id}")]
        public async Task<ActionResult<FlavorProfile?>> DeleteFlavorProfile(int id)
        {
            var deleted = await _mediator.Send(new DeleteFlavorProfileCommand(id));

            return Ok(deleted);
        }
    }
}
