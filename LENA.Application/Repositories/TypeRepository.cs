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
            entity.TypeID = await QuerySingleAsync<int>("[Wine].[usp_Type_Create]", entity, cancellationToken);
            return entity;
        }

        public override async Task<TypeEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<TypeEntity>("[Wine].[usp_Type_GetById]", new { Id = id }, cancellationToken);

        public override async Task<IReadOnlyList<TypeEntity>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<TypeEntity>("[Wine].[usp_Type_ListAll]", cancellationToken: cancellationToken);

        public override async Task<TypeEntity> UpdateAsync(TypeEntity entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Type_Update]", entity, cancellationToken);
            return entity;
        }

        public override async Task<TypeEntity> DeleteAsync(TypeEntity entitey, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Wine].[usp_Type_Delete]", new { entitey.TypeID }, cancellationToken);
            return entitey;
        }

        public override async Task<TypeEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<TypeEntity>("[Wine].[usp_Type_GetByName]", new { Name = name }, cancellationToken);
    }
}
