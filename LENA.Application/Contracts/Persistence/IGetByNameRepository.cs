namespace LENA.Application.Contracts.Persistence
{
    public interface IGetByNameRepository<T> where T : class
    {
        Task<T?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}