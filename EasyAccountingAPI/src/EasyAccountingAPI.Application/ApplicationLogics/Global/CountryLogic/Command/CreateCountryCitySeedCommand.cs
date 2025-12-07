namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class CreateCountryCitySeedCommand : IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCountryCitySeedCommand, bool>
        {
            private readonly ICountryManager _countryManager;
            private readonly ICityManager _cityManager;

            public Handler(ICountryManager countryManager, ICityManager cityManager)
            {
                _countryManager = countryManager;
                _cityManager = cityManager;
            }

            public async Task<bool> Handle(CreateCountryCitySeedCommand request, CancellationToken cancellationToken)
            {
                var faker = new Faker("en");

                // HashSets with capacity to reduce resizing cost
                var countryNameSet = new HashSet<string>(200, StringComparer.OrdinalIgnoreCase);
                var countryCodeSet = new HashSet<string>(200, StringComparer.OrdinalIgnoreCase);

                var countries = new List<Country>(200);

               for (int i = 0; i < 200; i++)
               {
                    var name = faker.Address.Country();
                    var code = faker.Address.CountryCode()?.Trim().ToLower() ?? "un";

                    if (!countryNameSet.Add(name)) continue;
                    if (!countryCodeSet.Add(code)) continue;

                    countries.Add(new Country
                    {
                        Name = name,
                        Code = code,
                        Icon = GetFlagCdnUrl(code),
                        IsDeleted = false
                    });
                }

                // Save countries
                var isSaveCountries = await _countryManager.BulkCreateAsync(countries) > 0;

                // Get saved country ids
                var countryIds = await _countryManager.GetCountryIdsAsync();

                // Unique city names
                var cityNameSet = new HashSet<string>(400, StringComparer.OrdinalIgnoreCase);
                var cities = new List<City>(400);

                for (int i = 0; i < 400; i++)
                {
                    var cityName = faker.Address.City();
                    if (!cityNameSet.Add(cityName)) continue;

                    cities.Add(new City
                    {
                        Name = cityName,
                        CountryId = faker.PickRandom(countryIds),
                        IsDeleted = false
                    });
                }

                // Save cities
                var isSaveCities = await _cityManager.BulkCreateAsync(cities) > 0;

                return isSaveCountries && isSaveCities;
            }

            // Get country flag URL from FlagCDN
            private static string GetFlagCdnUrl(string iso2Code)
            {
                // Always use lowercase two-letter code; adjust size as needed (w40/w80/w160)
                iso2Code = (iso2Code ?? "un").Trim().ToLowerInvariant();
                return $"https://flagcdn.com/w80/{iso2Code}.png";
            }
        }
    }
}