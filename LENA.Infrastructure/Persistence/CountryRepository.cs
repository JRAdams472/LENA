using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Infrastructure.Persistence
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<Country?> GetByISOCodeAsync(string isoCode, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Country>("[Wine].[usp_Country_GetByISOCode]", new { ISOCode = isoCode }, cancellationToken);

        public async Task<IReadOnlyList<Country>> GetAllActiveAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Country>("[Wine].[usp_Country_GetAllActive]", cancellationToken: cancellationToken);

        public override async Task<Country> CreateAsync(Country entity, CancellationToken cancellationToken = default)
        {
            entity.CountryID = await QuerySingleAsync<int>("[Wine].[usp_Country_Create]", new
            {
                entity.CountryName,
                entity.ISOCode,
                entity.Description,
                entity.IsActive,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Country>("[Wine].[usp_Country_GetById]", new { Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<Country>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Country>("[Wine].[usp_Country_ListAll]", cancellationToken: cancellationToken);

        public async Task<LENA.Application.Models.PagedResult<Country>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
            => await QueryPagedListAsync<Country>("[Wine].[usp_Country_ListAllPaged]", pageNumber, pageSize, ct: ct);

        public override async Task<Country> UpdateAsync(Country entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Country_Update]", new
            {
                entity.CountryID,
                entity.CountryName,
                entity.ISOCode,
                entity.Description,
                entity.IsActive,
                entity.LastUpdatedBy,
                entity.LastUpdatedDate
            }, nameof(Country), entity.CountryID, cancellationToken);
            return entity;
        }

        public override async Task<Country> DeleteAsync(Country entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Country_Delete]", new { CountryID = entity.CountryID }, nameof(Country), entity.CountryID, cancellationToken);
            return entity;
        }

        public async Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Country>("[Wine].[usp_Country_GetByName]", new { Name = name }, cancellationToken);
    }
}