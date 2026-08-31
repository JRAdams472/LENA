using System;
using System.Collections.Generic;
using System.Text;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.Contracts.Persistence
{
    public interface ITypeRepository : IWineRepository<TypeEntity>
    {
        Task<TypeEntity?> GetByNameAsync(string name);
    }
}