using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
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
            entity.CountryID = await QuerySingleAsync<int>("[Wine].[usp_Country_Create]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Country>("[Wine].[usp_Country_GetById]", new { Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<Country>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Country>("[Wine].[usp_Country_ListAll]", cancellationToken: cancellationToken);

        public override async Task<Country> UpdateAsync(Country entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Country_Update]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Country> DeleteAsync(Country entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Country_Delete]", new { CountryID = entity.CountryID }, cancellationToken);
            return entity;
        }

        public override async Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Country>("[Wine].[usp_Country_GetByName]", new { Name = name }, cancellationToken);
    }
}
