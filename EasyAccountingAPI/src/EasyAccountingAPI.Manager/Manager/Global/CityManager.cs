namespace EasyAccountingAPI.Manager.Manager.Global
{
    public class CityManager : BaseManager<City>, ICityManager
    {
        private readonly ICityRepository _cityRepository;

        public CityManager(ICityRepository cityRepository) : base(cityRepository) => _cityRepository = cityRepository;
    }
}