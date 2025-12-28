namespace EasyAccountingAPI.Repository.Contracts.Global
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<FilterPageResultModel<City>> GetCitiesByFilterAsync(FilterPageModel filterPageModel, CancellationToken cancellationToken);
        Task<int> DeleteBulkCityByCountryIdAsync(int countryId, CancellationToken cancellationToken);
    }
}