using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LENA.Application.Repositories
{
    public abstract class BaseWineRepository<T> : IAsyncRepository<T> where T : class
    {
        public virtual Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> DeleteAsync(T entitey)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IReadOnlyList<T>> ListAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}