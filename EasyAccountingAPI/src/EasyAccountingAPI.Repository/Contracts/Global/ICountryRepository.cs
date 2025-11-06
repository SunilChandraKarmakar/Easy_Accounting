namespace EasyAccountingAPI.Repository.Contracts.Global
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Task<FilterPagedResult<Country>> GetCountriesFilterAsync(int pageNumber, int pageSize);
    }
}