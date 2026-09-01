using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;

namespace LENA.Application.Repositories
{
    public class VintageRepository : BaseRepository<Vintage>, IVintageRepository
    {
        public VintageRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<Vintage> CreateAsync(Vintage entity, CancellationToken cancellationToken = default)
        {
            entity.VintageID = await QuerySingleAsync<int>("[Wine].[usp_Vintage_Create]", new
            {
                entity.Year,
                entity.Description,
                entity.IsActive,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<Vintage?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Vintage>("[Wine].[usp_Vintage_GetById]", new { Id = id }, cancellationToken);

        public override async Task<LENA.Application.Models.PagedResult<Vintage>> ListAllAsync(LENA.Application.Models.PaginationRequest? paging = null, CancellationToken cancellationToken = default)
            => await QueryPagedAsync<Vintage>("[Wine].[usp_Vintage_ListAll]", paging, cancellationToken);

        public async Task<LENA.Application.Models.PagedResult<Vintage>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
            => await QueryPagedListAsync<Vintage>("[Wine].[usp_Vintage_ListAllPaged]", pageNumber, pageSize, ct: ct);

        public override async Task<Vintage> UpdateAsync(Vintage entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Vintage_Update]", new
            {
                entity.VintageID,
                entity.Year,
                entity.Description,
                entity.IsActive,
                entity.LastUpdatedBy,
                entity.LastUpdatedDate
            }, nameof(Vintage), entity.VintageID, cancellationToken);
            return entity;
        }

        public override async Task<Vintage> DeleteAsync(Vintage entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Vintage_Delete]", new { entity.VintageID }, nameof(Vintage), entity.VintageID, cancellationToken);
            return entity;
        }

        public override Task<Vintage?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => Task.FromResult<Vintage?>(null);

        public async Task<Vintage?> GetByYearAsync(int year, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Vintage>("[Wine].[usp_Vintage_GetByYear]", new { Year = year }, cancellationToken);

        public async Task<IReadOnlyList<Vintage>> GetAllActiveAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Vintage>("[Wine].[usp_Vintage_GetAllActive]", cancellationToken: cancellationToken);
    }
}
