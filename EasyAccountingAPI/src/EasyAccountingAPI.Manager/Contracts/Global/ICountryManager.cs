namespace EasyAccountingAPI.Manager.Contracts.Global
{
    public interface ICountryManager : IBaseManager<Country>
    {
        Task<FilterPageResultModel<Country>> GetCountriesByFilterAsync(FilterPageModel filterPageModel);
        Task<IEnumerable<int>> GetCountryIdsAsync();
    }
}