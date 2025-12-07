namespace EasyAccountingAPI.Manager.Manager.Global
{
    public class CountryManager : BaseManager<Country>, ICountryManager
    {
        private readonly ICountryRepository _countryRepository;

        public CountryManager(ICountryRepository countryRepository) : base(countryRepository) 
            => _countryRepository = countryRepository;

        public async Task<FilterPageResultModel<Country>> GetCountriesByFilterAsync(FilterPageModel filterPageModel)
        {
            return await _countryRepository.GetCountriesByFilterAsync(filterPageModel);
        }

        public async Task<IEnumerable<int>> GetCountryIdsAsync()
        {
            return await _countryRepository.GetCountryIdsAsync();
        }
    }
}