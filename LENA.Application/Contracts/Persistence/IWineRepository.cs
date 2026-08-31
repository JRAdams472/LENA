namespace LENA.Application.Contracts.Persistence
{
    public interface IWineRepository<T> : IAsyncRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllByCountryIdAsync(int countryId);
        Task<IReadOnlyList<T>> GetAllByRegionIdAsync(int regionId);
        Task<IReadOnlyList<T>> GetAllByTypeIdAsync(int typeId);
        Task<IReadOnlyList<T>> GetAllByVintageYearAsync(int vintageYear);
    }
}