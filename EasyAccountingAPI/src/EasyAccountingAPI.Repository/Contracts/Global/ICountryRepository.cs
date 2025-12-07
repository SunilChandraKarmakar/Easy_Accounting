namespace EasyAccountingAPI.Repository.Contracts.Global
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Task<FilterPageResultModel<Country>> GetCountriesByFilterAsync(FilterPageModel filterPageModel);
        Task<IEnumerable<int>> GetCountryIdsAsync();
    }
}