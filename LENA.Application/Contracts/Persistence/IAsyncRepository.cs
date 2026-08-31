using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T?> GetByIdAsync(int Id);
        Task<T?> GetByNameAsync(string Name);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entitey);
        Task<IReadOnlyList<T>> ListAllAsync();
    }
}
