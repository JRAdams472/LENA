using LENA.Application.Features.Wine.Commands;
using LENA.Application.Features.Wine.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Bottle endpoints
        [HttpGet("bottles")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottles()
        {
            var bottles = await _mediator.Send(new GetBottlesQuery());
            return Ok(bottles);
        }

        [HttpGet("bottles/{id}")]
        public async Task<ActionResult<Bottle?>> GetBottleById(int id)
        {
            var bottle = await _mediator.Send(new GetBottleByIdQuery(id));
            if (bottle == null)
                return NotFound();

            return Ok(bottle);
        }

        [HttpGet("bottles/country/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByCountryId(int countryId)
        {
            var bottles = await _mediator.Send(new GetBottlesByCountryIdQuery(countryId));
            return Ok(bottles);
        }

        [HttpGet("bottles/region/{regionId}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByRegionId(int regionId)
        {
            var bottles = await _mediator.Send(new GetBottlesByRegionIdQuery(regionId));
            return Ok(bottles);
        }

        [HttpGet("bottles/type/{typeId}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByTypeId(int typeId)
        {
            var bottles = await _mediator.Send(new GetBottlesByTypeIdQuery(typeId));
            return Ok(bottles);
        }

        [HttpGet("bottles/vintage/{vintageYear}")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetBottlesByVintageYear(int vintageYear)
        {
            var bottles = await _mediator.Send(new GetBottlesByVintageYearQuery(vintageYear));
            return Ok(bottles);
        }

        [HttpGet("bottles/favorites")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> GetFavoriteBottles()
        {
            var bottles = await _mediator.Send(new GetFavoriteBottlesQuery());
            return Ok(bottles);
        }

        [HttpGet("bottles/search")]
        public async Task<ActionResult<IReadOnlyList<Bottle>>> SearchBottles([FromQuery] string searchTerm)
        {
            var bottles = await _mediator.Send(new SearchBottlesQuery(searchTerm));
            return Ok(bottles);
        }

        [HttpGet("bottles/count")]
        public async Task<ActionResult<int>> GetTotalBottleCount()
        {
            var count = await _mediator.Send(new GetTotalBottleCountQuery());
            return Ok(count);
        }

        [HttpPost("bottles")]
        public async Task<ActionResult<Bottle>> CreateBottle([FromBody] Bottle bottle)
        {
            var created = await _mediator.Send(new CreateBottleCommand(bottle));
            return CreatedAtAction(nameof(GetBottleById), new { id = created.BottleID }, created);
        }

        [HttpPut("bottles/{id}")]
        public async Task<ActionResult<Bottle>> UpdateBottle(int id, [FromBody] Bottle bottle)
        {
            if (id != bottle.BottleID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateBottleCommand(bottle));
            return Ok(updated);
        }

        [HttpDelete("bottles/{id}")]
        public async Task<ActionResult<Bottle?>> DeleteBottle(int id)
        {
            var deleted = await _mediator.Send(new DeleteBottleCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }

        // Country endpoints
        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetCountries()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("countries/active")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetActiveCountries()
        {
            var countries = await _mediator.Send(new GetActiveCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("countries/{id}")]
        public async Task<ActionResult<Country?>> GetCountryById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id));
            if (country == null)
                return NotFound();

            return Ok(country);
        }

        [HttpGet("countries/iso/{isoCode}")]
        public async Task<ActionResult<Country?>> GetCountryByISOCode(string isoCode)
        {
            var country = await _mediator.Send(new GetCountryByISOCodeQuery(isoCode));
            if (country == null)
                return NotFound();

            return Ok(country);
        }

        [HttpPost("countries")]
        public async Task<ActionResult<Country>> CreateCountry([FromBody] Country country)
        {
            var created = await _mediator.Send(new CreateCountryCommand(country));
            return CreatedAtAction(nameof(GetCountryById), new { id = created.CountryID }, created);
        }

        [HttpPut("countries/{id}")]
        public async Task<ActionResult<Country>> UpdateCountry(int id, [FromBody] Country country)
        {
            if (id != country.CountryID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateCountryCommand(country));
            return Ok(updated);
        }

        [HttpDelete("countries/{id}")]
        public async Task<ActionResult<Country?>> DeleteCountry(int id)
        {
            var deleted = await _mediator.Send(new DeleteCountryCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }

        // Region endpoints
        [HttpGet("regions")]
        public async Task<ActionResult<IReadOnlyList<Region>>> GetRegions()
        {
            var regions = await _mediator.Send(new GetRegionsQuery());
            return Ok(regions);
        }

        [HttpGet("regions/country/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<Region>>> GetRegionsByCountryId(int countryId)
        {
            var regions = await _mediator.Send(new GetRegionsByCountryIdQuery(countryId));
            return Ok(regions);
        }

        [HttpGet("regions/{id}")]
        public async Task<ActionResult<Region?>> GetRegionById(int id)
        {
            var region = await _mediator.Send(new GetRegionByIdQuery(id));
            if (region == null)
                return NotFound();

            return Ok(region);
        }

        [HttpGet("regions/by-name")]
        public async Task<ActionResult<Region?>> GetRegionByNameAndCountryId([FromQuery] string name, [FromQuery] int countryId)
        {
            var region = await _mediator.Send(new GetRegionByNameAndCountryIdQuery(name, countryId));
            if (region == null)
                return NotFound();

            return Ok(region);
        }

        [HttpPost("regions")]
        public async Task<ActionResult<Region>> CreateRegion([FromBody] Region region)
        {
            var created = await _mediator.Send(new CreateRegionCommand(region));
            return CreatedAtAction(nameof(GetRegionById), new { id = created.RegionID }, created);
        }

        [HttpPut("regions/{id}")]
        public async Task<ActionResult<Region>> UpdateRegion(int id, [FromBody] Region region)
        {
            if (id != region.RegionID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateRegionCommand(region));
            return Ok(updated);
        }

        [HttpDelete("regions/{id}")]
        public async Task<ActionResult<Region?>> DeleteRegion(int id)
        {
            var deleted = await _mediator.Send(new DeleteRegionCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }

        // Type endpoints
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeEntity>>> GetTypes()
        {
            var types = await _mediator.Send(new GetTypesQuery());
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

        // Vintage endpoints
        [HttpGet("vintages")]
        public async Task<ActionResult<IReadOnlyList<Vintage>>> GetVintages()
        {
            var vintages = await _mediator.Send(new GetVintagesQuery());
            return Ok(vintages);
        }

        [HttpGet("vintages/active")]
        public async Task<ActionResult<IReadOnlyList<Vintage>>> GetActiveVintages()
        {
            var vintages = await _mediator.Send(new GetActiveVintagesQuery());
            return Ok(vintages);
        }

        [HttpGet("vintages/{id}")]
        public async Task<ActionResult<Vintage?>> GetVintageById(int id)
        {
            var vintage = await _mediator.Send(new GetVintageByIdQuery(id));
            if (vintage == null)
                return NotFound();

            return Ok(vintage);
        }

        [HttpGet("vintages/year/{year}")]
        public async Task<ActionResult<Vintage?>> GetVintageByYear(int year)
        {
            var vintage = await _mediator.Send(new GetVintageByYearQuery(year));
            if (vintage == null)
                return NotFound();

            return Ok(vintage);
        }

        [HttpPost("vintages")]
        public async Task<ActionResult<Vintage>> CreateVintage([FromBody] Vintage vintage)
        {
            var created = await _mediator.Send(new CreateVintageCommand(vintage));
            return CreatedAtAction(nameof(GetVintageById), new { id = created.VintageID }, created);
        }

        [HttpPut("vintages/{id}")]
        public async Task<ActionResult<Vintage>> UpdateVintage(int id, [FromBody] Vintage vintage)
        {
            if (id != vintage.VintageID)
                return BadRequest();

            var updated = await _mediator.Send(new UpdateVintageCommand(vintage));
            return Ok(updated);
        }

        [HttpDelete("vintages/{id}")]
        public async Task<ActionResult<Vintage?>> DeleteVintage(int id)
        {
            var deleted = await _mediator.Send(new DeleteVintageCommand(id));
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }
}
