using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Application.Contracts.Persistence
{
    public interface ITypeRepository : IWineRepository<Type>
    {
        Task<Type> GetByNameAsync(string name);
    }
}