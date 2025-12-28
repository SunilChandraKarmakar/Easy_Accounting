namespace EasyAccountingAPI.Repository.Contracts.Global
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<FilterPageResultModel<City>> GetCitiesByFilterAsync(FilterPageModel filterPageModel);
        Task<int> DeleteBulkCityByCountryIdAsync(int countryId);
    }
}