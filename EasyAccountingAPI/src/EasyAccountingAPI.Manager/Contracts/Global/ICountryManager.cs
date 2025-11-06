namespace EasyAccountingAPI.Manager.Contracts.Global
{
    public interface ICountryManager : IBaseManager<Country>
    {
        Task<FilterPagedResult<Country>> GetCountriesFilterAsync(int pageNumber, int pageSize);
    }
}