namespace EasyAccountingAPI.Manager.Contracts.Global
{
    public interface ICityManager : IBaseManager<City>
    {
        Task<int> DeleteBulkCityByCountryIdAsync(int countryId);
    }
}