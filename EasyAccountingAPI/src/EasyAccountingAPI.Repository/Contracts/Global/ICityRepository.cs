namespace EasyAccountingAPI.Repository.Contracts.Global
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<int> DeleteBulkCityByCountryIdAsync(int countryId);
    }
}