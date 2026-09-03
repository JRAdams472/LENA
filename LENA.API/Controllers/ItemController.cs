using LENA.API.Contracts.Inventory;
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
        [Obsolete("Use GET /api/Item/items/paged instead.")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<ItemResponse>>> GetItems()
        {
            var paged = await _mediator.Send(new GetItemsPagedQuery(1, 25, null, null, false, false));
            return Ok(new LENA.Application.Models.PagedResult<ItemResponse>
            {
                Items = paged.Items.Select(ItemResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("items/search")]
        public async Task<ActionResult<IReadOnlyList<ItemResponse>>> SearchItems([FromQuery] string? search = null, [FromQuery] string? brand = null, [FromQuery] int limit = 50)
        {
            if (string.IsNullOrWhiteSpace(brand) && (string.IsNullOrWhiteSpace(search) || search!.Length < 2))
                return Ok(Array.Empty<ItemResponse>());

            var items = await _mediator.Send(new SearchItemsQuery(search ?? string.Empty, brand, limit));
            return Ok(items.Select(ItemResponse.FromEntity).ToList());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands([FromQuery] string? search = null)
        {
            var brands = await _mediator.Send(new GetItemBrandsQuery(search));
            return Ok(brands);
        }

        [HttpGet("items/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<ItemResponse>>> GetItemsPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string? search = null,
            [FromQuery] string? brand = null,
            [FromQuery] bool inStock = false,
            [FromQuery] bool isFavorite = false)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetItemsPagedQuery(pageNumber, pageSize, search, brand, inStock, isFavorite));
            return Ok(new LENA.Application.Models.PagedResult<ItemResponse>
            {
                Items = paged.Items.Select(ItemResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<ItemResponse?>> GetItemById(int id)
        {
            var item = await _mediator.Send(new GetItemByIdQuery(id));
            return Ok(ItemResponse.FromEntity(item!));
        }

        [HttpGet("items/name/{name}")]
        public async Task<ActionResult<ItemResponse?>> GetItemByName(string name)
        {
            var item = await _mediator.Send(new GetItemByNameQuery(name));
            return Ok(ItemResponse.FromEntity(item!));
        }

        [HttpPost("items")]
        public async Task<ActionResult<ItemResponse>> CreateItem([FromBody] CreateItemRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateItemCommand(entity));
            return CreatedAtAction(nameof(GetItemById), new { id = created.ItemID }, ItemResponse.FromEntity(created!));
        }

        [HttpPut("items/{id}")]
        public async Task<ActionResult<ItemResponse>> UpdateItem(int id, [FromBody] UpdateItemRequest request)
        {
            if (id != request.ItemID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateItemCommand(entity));
            return Ok(ItemResponse.FromEntity(updated!));
        }

        [HttpDelete("items/{id}")]
        public async Task<ActionResult<ItemResponse?>> DeleteItem(int id)
        {
            var deleted = await _mediator.Send(new DeleteItemCommand(id));
            return Ok(ItemResponse.FromEntity(deleted!));
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
        public async Task<ActionResult<IReadOnlyList<FoodFlavorResponse>>> GetFoodFlavors()
        {
            var foodFlavors = await _mediator.Send(new GetFoodFlavorsQuery());
            return Ok(foodFlavors.Select(FoodFlavorResponse.FromEntity).ToList());
        }

        [HttpGet("foodflavors/{id}")]
        public async Task<ActionResult<FoodFlavorResponse?>> GetFoodFlavorById(int id)
        {
            var foodFlavor = await _mediator.Send(new GetFoodFlavorByIdQuery(id));
            return Ok(FoodFlavorResponse.FromEntity(foodFlavor!));
        }

        [HttpGet("foodflavors/food/{foodId}")]
        public async Task<ActionResult<IReadOnlyList<FoodFlavorResponse>>> GetFoodFlavorsByFoodId(int foodId)
        {
            var foodFlavors = await _mediator.Send(new GetFoodFlavorsByFoodIdQuery(foodId));
            return Ok(foodFlavors.Select(FoodFlavorResponse.FromEntity).ToList());
        }

        [HttpGet("foodflavors/flavor/{flavorId}")]
        public async Task<ActionResult<IReadOnlyList<FoodFlavorResponse>>> GetFoodFlavorsByFlavorId(int flavorId)
        {
            var foodFlavors = await _mediator.Send(new GetFoodFlavorsByFlavorIdQuery(flavorId));
            return Ok(foodFlavors.Select(FoodFlavorResponse.FromEntity).ToList());
        }

        [HttpGet("foodflavors/food/{foodId}/flavor/{flavorId}")]
        public async Task<ActionResult<FoodFlavorResponse?>> GetFoodFlavorByFoodAndFlavorId(int foodId, int flavorId)
        {
            var foodFlavor = await _mediator.Send(new GetFoodFlavorByFoodAndFlavorIdQuery(foodId, flavorId));
            return Ok(FoodFlavorResponse.FromEntity(foodFlavor!));
        }

        [HttpPost("foodflavors")]
        public async Task<ActionResult<FoodFlavorResponse>> CreateFoodFlavor([FromBody] CreateFoodFlavorRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateFoodFlavorCommand(entity));
            return CreatedAtAction(nameof(GetFoodFlavorByFoodAndFlavorId), new { foodId = created.FoodId, flavorId = created.FlavorId }, FoodFlavorResponse.FromEntity(created!));
        }

        [HttpPut("foodflavors/food/{foodId}/flavor/{flavorId}")]
        public async Task<ActionResult<FoodFlavorResponse>> UpdateFoodFlavor(int foodId, int flavorId, [FromBody] UpdateFoodFlavorRequest request)
        {
            if (foodId != request.FoodId || flavorId != request.FlavorId)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateFoodFlavorCommand(entity));
            return Ok(FoodFlavorResponse.FromEntity(updated!));
        }

        [HttpDelete("foodflavors/food/{foodId}/flavor/{flavorId}")]
        public async Task<ActionResult<FoodFlavorResponse?>> DeleteFoodFlavor(int foodId, int flavorId)
        {
            var deleted = await _mediator.Send(new DeleteFoodFlavorCommand(foodId, flavorId));
            return Ok(FoodFlavorResponse.FromEntity(deleted!));
        }

        // FoodNutrient endpoints
        [HttpGet("foodnutrients")]
        public async Task<ActionResult<IReadOnlyList<FoodNutrientResponse>>> GetFoodNutrients()
        {
            var foodNutrients = await _mediator.Send(new GetFoodNutrientsQuery());
            return Ok(foodNutrients.Select(FoodNutrientResponse.FromEntity).ToList());
        }

        [HttpGet("foodnutrients/{id}")]
        public async Task<ActionResult<FoodNutrientResponse?>> GetFoodNutrientById(int id)
        {
            var foodNutrient = await _mediator.Send(new GetFoodNutrientByIdQuery(id));
            return Ok(FoodNutrientResponse.FromEntity(foodNutrient!));
        }

        [HttpGet("foodnutrients/food/{foodId}")]
        public async Task<ActionResult<IReadOnlyList<FoodNutrientResponse>>> GetFoodNutrientsByFoodId(int foodId)
        {
            var foodNutrients = await _mediator.Send(new GetFoodNutrientsByFoodIdQuery(foodId));
            return Ok(foodNutrients.Select(FoodNutrientResponse.FromEntity).ToList());
        }

        [HttpGet("foodnutrients/nutrient/{nutrientId}")]
        public async Task<ActionResult<IReadOnlyList<FoodNutrientResponse>>> GetFoodNutrientsByNutrientId(int nutrientId)
        {
            var foodNutrients = await _mediator.Send(new GetFoodNutrientsByNutrientIdQuery(nutrientId));
            return Ok(foodNutrients.Select(FoodNutrientResponse.FromEntity).ToList());
        }

        [HttpGet("foodnutrients/food/{foodId}/nutrient/{nutrientId}")]
        public async Task<ActionResult<FoodNutrientResponse?>> GetFoodNutrientByFoodAndNutrientId(int foodId, int nutrientId)
        {
            var foodNutrient = await _mediator.Send(new GetFoodNutrientByFoodAndNutrientIdQuery(foodId, nutrientId));
            return Ok(FoodNutrientResponse.FromEntity(foodNutrient!));
        }

        [HttpPost("foodnutrients")]
        public async Task<ActionResult<FoodNutrientResponse>> CreateFoodNutrient([FromBody] CreateFoodNutrientRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateFoodNutrientCommand(entity));
            return CreatedAtAction(nameof(GetFoodNutrientByFoodAndNutrientId), new { foodId = created.FoodId, nutrientId = created.NutrientId }, FoodNutrientResponse.FromEntity(created!));
        }

        [HttpPut("foodnutrients/food/{foodId}/nutrient/{nutrientId}")]
        public async Task<ActionResult<FoodNutrientResponse>> UpdateFoodNutrient(int foodId, int nutrientId, [FromBody] UpdateFoodNutrientRequest request)
        {
            if (foodId != request.FoodId || nutrientId != request.NutrientId)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateFoodNutrientCommand(entity));
            return Ok(FoodNutrientResponse.FromEntity(updated!));
        }

        [HttpDelete("foodnutrients/food/{foodId}/nutrient/{nutrientId}")]
        public async Task<ActionResult<FoodNutrientResponse?>> DeleteFoodNutrient(int foodId, int nutrientId)
        {
            var deleted = await _mediator.Send(new DeleteFoodNutrientCommand(foodId, nutrientId));
            return Ok(FoodNutrientResponse.FromEntity(deleted!));
        }

        // NutrientType endpoints
        [HttpGet("nutrienttypes")]
        public async Task<ActionResult<IReadOnlyList<NutrientTypeResponse>>> GetNutrientTypes()
        {
            var nutrientTypes = await _mediator.Send(new GetNutrientTypesQuery());
            return Ok(nutrientTypes.Select(NutrientTypeResponse.FromEntity).ToList());
        }

        [HttpGet("nutrienttypes/{id}")]
        public async Task<ActionResult<NutrientTypeResponse?>> GetNutrientTypeById(int id)
        {
            var nutrientType = await _mediator.Send(new GetNutrientTypeByIdQuery(id));
            return Ok(NutrientTypeResponse.FromEntity(nutrientType!));
        }

        [HttpGet("nutrienttypes/name/{name}")]
        public async Task<ActionResult<NutrientTypeResponse?>> GetNutrientTypeByName(string name)
        {
            var nutrientType = await _mediator.Send(new GetNutrientTypeByNameQuery(name));
            return Ok(NutrientTypeResponse.FromEntity(nutrientType!));
        }

        [HttpPost("nutrienttypes")]
        public async Task<ActionResult<NutrientTypeResponse>> CreateNutrientType([FromBody] CreateNutrientTypeRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateNutrientTypeCommand(entity));
            return CreatedAtAction(nameof(GetNutrientTypeById), new { id = created.NutrientId }, NutrientTypeResponse.FromEntity(created!));
        }

        [HttpPut("nutrienttypes/{id}")]
        public async Task<ActionResult<NutrientTypeResponse>> UpdateNutrientType(int id, [FromBody] UpdateNutrientTypeRequest request)
        {
            if (id != request.NutrientId)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateNutrientTypeCommand(entity));
            return Ok(NutrientTypeResponse.FromEntity(updated!));
        }

        [HttpDelete("nutrienttypes/{id}")]
        public async Task<ActionResult<NutrientTypeResponse?>> DeleteNutrientType(int id)
        {
            var deleted = await _mediator.Send(new DeleteNutrientTypeCommand(id));
            return Ok(NutrientTypeResponse.FromEntity(deleted!));
        }

        // FlavorProfile endpoints
        [HttpGet("flavorprofiles")]
        public async Task<ActionResult<IReadOnlyList<FlavorProfileResponse>>> GetFlavorProfiles()
        {
            var flavorProfiles = await _mediator.Send(new GetFlavorProfilesQuery());
            return Ok(flavorProfiles.Select(FlavorProfileResponse.FromEntity).ToList());
        }

        [HttpGet("flavorprofiles/active")]
        public async Task<ActionResult<IReadOnlyList<FlavorProfileResponse>>> GetActiveFlavorProfiles()
        {
            var flavorProfiles = await _mediator.Send(new GetActiveFlavorProfilesQuery());
            return Ok(flavorProfiles.Select(FlavorProfileResponse.FromEntity).ToList());
        }

        [HttpGet("flavorprofiles/{id}")]
        public async Task<ActionResult<FlavorProfileResponse?>> GetFlavorProfileById(int id)
        {
            var flavorProfile = await _mediator.Send(new GetFlavorProfileByIdQuery(id));
            return Ok(FlavorProfileResponse.FromEntity(flavorProfile!));
        }

        [HttpGet("flavorprofiles/name/{name}")]
        public async Task<ActionResult<FlavorProfileResponse?>> GetFlavorProfileByName(string name)
        {
            var flavorProfile = await _mediator.Send(new GetFlavorProfileByNameQuery(name));
            return Ok(FlavorProfileResponse.FromEntity(flavorProfile!));
        }

        [HttpPost("flavorprofiles")]
        public async Task<ActionResult<FlavorProfileResponse>> CreateFlavorProfile([FromBody] CreateFlavorProfileRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateFlavorProfileCommand(entity));
            return CreatedAtAction(nameof(GetFlavorProfileById), new { id = created.FlavorId }, FlavorProfileResponse.FromEntity(created!));
        }

        [HttpPut("flavorprofiles/{id}")]
        public async Task<ActionResult<FlavorProfileResponse>> UpdateFlavorProfile(int id, [FromBody] UpdateFlavorProfileRequest request)
        {
            if (id != request.FlavorId)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateFlavorProfileCommand(entity));
            return Ok(FlavorProfileResponse.FromEntity(updated!));
        }

        [HttpDelete("flavorprofiles/{id}")]
        public async Task<ActionResult<FlavorProfileResponse?>> DeleteFlavorProfile(int id)
        {
            var deleted = await _mediator.Send(new DeleteFlavorProfileCommand(id));
            return Ok(FlavorProfileResponse.FromEntity(deleted!));
        }
    }
}