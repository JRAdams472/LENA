using LENA.Application.Contracts.Persistence;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Repositories
{
    public class TypeRepository : BaseRepository<TypeEntity>, ITypeRepository
    {
        public TypeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<TypeEntity> CreateAsync(TypeEntity entity, CancellationToken cancellationToken = default)
        {
            entity.TypeID = await QuerySingleAsync<int>("[Wine].[usp_Type_Create]", new
            {
                entity.TypeName,
                entity.Description,
                entity.IsActive,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<TypeEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<TypeEntity>("[Wine].[usp_Type_GetById]", new { Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<TypeEntity>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<TypeEntity>("[Wine].[usp_Type_ListAll]", cancellationToken: cancellationToken);

        public async Task<LENA.Application.Models.PagedResult<TypeEntity>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
            => await QueryPagedListAsync<TypeEntity>("[Wine].[usp_Type_ListAllPaged]", pageNumber, pageSize, ct: ct);

        public override async Task<TypeEntity> UpdateAsync(TypeEntity entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Type_Update]", new
            {
                entity.TypeID,
                entity.TypeName,
                entity.Description,
                entity.IsActive,
                entity.LastUpdatedBy,
                entity.LastUpdatedDate
            }, nameof(TypeEntity), entity.TypeID, cancellationToken);
            return entity;
        }

        public override async Task<TypeEntity> DeleteAsync(TypeEntity entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Wine].[usp_Type_Delete]", new { entity.TypeID }, nameof(TypeEntity), entity.TypeID, cancellationToken);
            return entity;
        }

        public async Task<TypeEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<TypeEntity>("[Wine].[usp_Type_GetByName]", new { Name = name }, cancellationToken);
    }
}
