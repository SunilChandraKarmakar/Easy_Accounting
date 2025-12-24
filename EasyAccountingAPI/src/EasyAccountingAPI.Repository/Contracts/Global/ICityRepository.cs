namespace EasyAccountingAPI.Repository.Contracts.Global
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<bool> DeleteBulkCityByCountryIdAsync(int countryId);
    }
}