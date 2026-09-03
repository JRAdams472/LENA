import pathlib, os

root = pathlib.Path('c:/Users/aipal/OneDrive/WIP/LENA')
ctrl_dir = root / 'LENA.API' / 'Controllers'

files = {
    'CountryController.cs': """using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Application.Features.Wine.Countries.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<CountryResponse>>> GetCountries()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            return Ok(countries.Select(CountryResponse.FromEntity));
        }

        [HttpGet("countries/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<CountryResponse>>> GetCountriesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetCountriesPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<CountryResponse>
            {
                Items = paged.Items.Select(CountryResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("countries/active")]
        public async Task<ActionResult<IReadOnlyList<CountryResponse>>> GetActiveCountries()
        {
            var countries = await _mediator.Send(new GetActiveCountriesQuery());
            return Ok(countries.Select(CountryResponse.FromEntity));
        }

        [HttpGet("countries/{id}")]
        public async Task<ActionResult<CountryResponse?>> GetCountryById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id));
            return Ok(CountryResponse.FromEntity(country));
        }

        [HttpGet("countries/iso/{isoCode}")]
        public async Task<ActionResult<CountryResponse?>> GetCountryByISOCode(string isoCode)
        {
            var country = await _mediator.Send(new GetCountryByISOCodeQuery(isoCode));
            return Ok(CountryResponse.FromEntity(country));
        }

        [HttpPost("countries")]
        public async Task<ActionResult<CountryResponse>> CreateCountry([FromBody] CreateCountryRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateCountryCommand(entity));
            return CreatedAtAction(nameof(GetCountryById), new { id = created.CountryID }, CountryResponse.FromEntity(created));
        }

        [HttpPut("countries/{id}")]
        public async Task<ActionResult<CountryResponse>> UpdateCountry(int id, [FromBody] UpdateCountryRequest request)
        {
            if (id != request.CountryID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateCountryCommand(entity));
            return Ok(CountryResponse.FromEntity(updated));
        }

        [HttpDelete("countries/{id}")]
        public async Task<ActionResult<CountryResponse?>> DeleteCountry(int id)
        {
            var deleted = await _mediator.Send(new DeleteCountryCommand(id));
            return Ok(CountryResponse.FromEntity(deleted));
        }
    }
}
""",
    'RegionController.cs': """using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Regions.Commands;
using LENA.Application.Features.Wine.Regions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class RegionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("regions")]
        public async Task<ActionResult<IReadOnlyList<RegionResponse>>> GetRegions()
        {
            var regions = await _mediator.Send(new GetRegionsQuery());
            return Ok(regions.Select(RegionResponse.FromEntity));
        }

        [HttpGet("regions/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<RegionResponse>>> GetRegionsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetRegionsPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<RegionResponse>
            {
                Items = paged.Items.Select(RegionResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("regions/country/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<RegionResponse>>> GetRegionsByCountryId(int countryId)
        {
            var regions = await _mediator.Send(new GetRegionsByCountryIdQuery(countryId));
            return Ok(regions.Select(RegionResponse.FromEntity));
        }

        [HttpGet("regions/{id}")]
        public async Task<ActionResult<RegionResponse?>> GetRegionById(int id)
        {
            var region = await _mediator.Send(new GetRegionByIdQuery(id));
            return Ok(RegionResponse.FromEntity(region));
        }

        [HttpGet("regions/by-name")]
        public async Task<ActionResult<RegionResponse?>> GetRegionByNameAndCountryId([FromQuery] string name, [FromQuery] int countryId)
        {
            var region = await _mediator.Send(new GetRegionByNameAndCountryIdQuery(name, countryId));
            return Ok(RegionResponse.FromEntity(region));
        }

        [HttpPost("regions")]
        public async Task<ActionResult<RegionResponse>> CreateRegion([FromBody] CreateRegionRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateRegionCommand(entity));
            return CreatedAtAction(nameof(GetRegionById), new { id = created.RegionID }, RegionResponse.FromEntity(created));
        }

        [HttpPut("regions/{id}")]
        public async Task<ActionResult<RegionResponse>> UpdateRegion(int id, [FromBody] UpdateRegionRequest request)
        {
            if (id != request.RegionID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateRegionCommand(entity));
            return Ok(RegionResponse.FromEntity(updated));
        }

        [HttpDelete("regions/{id}")]
        public async Task<ActionResult<RegionResponse?>> DeleteRegion(int id)
        {
            var deleted = await _mediator.Send(new DeleteRegionCommand(id));
            return Ok(RegionResponse.FromEntity(deleted));
        }
    }
}
""",
    'VintageController.cs': """using LENA.API.Contracts.Wine;
using LENA.Application.Features.Wine.Vintages.Commands;
using LENA.Application.Features.Wine.Vintages.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LENA.API.Controllers
{
    [ApiController]
    [Route("api/Wine")]
    public class VintageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VintageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("vintages")]
        public async Task<ActionResult<IReadOnlyList<VintageResponse>>> GetVintages()
        {
            var vintages = await _mediator.Send(new GetVintagesQuery());
            return Ok(vintages.Select(VintageResponse.FromEntity));
        }

        [HttpGet("vintages/paged")]
        public async Task<ActionResult<LENA.Application.Models.PagedResult<VintageResponse>>> GetVintagesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            (pageNumber, pageSize) = LENA.Application.Models.PaginationRequest.Clamp(pageNumber, pageSize);
            var paged = await _mediator.Send(new GetVintagesPagedQuery(pageNumber, pageSize));
            return Ok(new LENA.Application.Models.PagedResult<VintageResponse>
            {
                Items = paged.Items.Select(VintageResponse.FromEntity).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
            });
        }

        [HttpGet("vintages/active")]
        public async Task<ActionResult<IReadOnlyList<VintageResponse>>> GetActiveVintages()
        {
            var vintages = await _mediator.Send(new GetActiveVintagesQuery());
            return Ok(vintages.Select(VintageResponse.FromEntity));
        }

        [HttpGet("vintages/{id}")]
        public async Task<ActionResult<VintageResponse?>> GetVintageById(int id)
        {
            var vintage = await _mediator.Send(new GetVintageByIdQuery(id));
            return Ok(VintageResponse.FromEntity(vintage));
        }

        [HttpGet("vintages/year/{year}")]
        public async Task<ActionResult<VintageResponse?>> GetVintageByYear(int year)
        {
            var vintage = await _mediator.Send(new GetVintageByYearQuery(year));
            return Ok(VintageResponse.FromEntity(vintage));
        }

        [HttpPost("vintages")]
        public async Task<ActionResult<VintageResponse>> CreateVintage([FromBody] CreateVintageRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateVintageCommand(entity));
            return CreatedAtAction(nameof(GetVintageById), new { id = created.VintageID }, VintageResponse.FromEntity(created));
        }

        [HttpPut("vintages/{id}")]
        public async Task<ActionResult<VintageResponse>> UpdateVintage(int id, [FromBody] UpdateVintageRequest request)
        {
            if (id != request.VintageID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateVintageCommand(entity));
            return Ok(VintageResponse.FromEntity(updated));
        }

        [HttpDelete("vintages/{id}")]
        public async Task<ActionResult<VintageResponse?>> DeleteVintage(int id)
        {
            var deleted = await _mediator.Send(new DeleteVintageCommand(id));
            return Ok(VintageResponse.FromEntity(deleted));
        }
    }
}
""",
    'WineTypeController.cs': """using LENA.API.Contracts.Wine;
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
            return Ok(TypeResponse.FromEntity(type));
        }

        [HttpGet("types/name/{name}")]
        public async Task<ActionResult<TypeResponse?>> GetTypeByName(string name)
        {
            var type = await _mediator.Send(new GetTypeByNameQuery(name));
            return Ok(TypeResponse.FromEntity(type));
        }

        [HttpPost("types")]
        public async Task<ActionResult<TypeResponse>> CreateType([FromBody] CreateTypeRequest request)
        {
            var entity = request.ToEntity();
            var created = await _mediator.Send(new CreateTypeCommand(entity));
            return CreatedAtAction(nameof(GetTypeById), new { id = created.TypeID }, TypeResponse.FromEntity(created));
        }

        [HttpPut("types/{id}")]
        public async Task<ActionResult<TypeResponse>> UpdateType(int id, [FromBody] UpdateTypeRequest request)
        {
            if (id != request.TypeID)
                return BadRequest();

            var entity = request.ToEntity();
            var updated = await _mediator.Send(new UpdateTypeCommand(entity));
            return Ok(TypeResponse.FromEntity(updated));
        }

        [HttpDelete("types/{id}")]
        public async Task<ActionResult<TypeResponse?>> DeleteType(int id)
        {
            var deleted = await _mediator.Send(new DeleteTypeCommand(id));
            return Ok(TypeResponse.FromEntity(deleted));
        }
    }
}
""",
}

for f, content in files.items():
    path = ctrl_dir / f
    if path.exists():
        path.unlink()
    path.write_text(content, encoding='utf-8', newline='')
    print('wrote', path)
