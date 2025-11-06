namespace EasyAccountingAPI.Manager.Manager.Global
{
    public class CountryManager : BaseManager<Country>, ICountryManager
    {
        private readonly ICountryRepository _countryRepository;

        public CountryManager(ICountryRepository countryRepository) : base(countryRepository) 
            => _countryRepository = countryRepository;

        public async Task<FilterPagedResult<Country>> GetCountriesFilterAsync(int pageNumber, int pageSize)
        {
            return await _countryRepository.GetCountriesFilterAsync(pageNumber, pageSize);
        }
    }
}