namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class CreateCountryCitySeedCommand : IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCountryCitySeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICountryRepository _countryRepository;
            private readonly ICityRepository _cityRepository;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICountryRepository countryRepository, ICityRepository cityRepository)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _countryRepository = countryRepository;
                _cityRepository = cityRepository;
            }

            public async Task<bool> Handle(CreateCountryCitySeedCommand request, CancellationToken cancellationToken)
            {
                var faker = new Faker("en");

                var countryNameSet = new HashSet<string>(200, StringComparer.OrdinalIgnoreCase);
                var countryCodeSet = new HashSet<string>(200, StringComparer.OrdinalIgnoreCase);

                var countries = new List<Country>(200);

                for (int i = 0; i < 200; i++)
                {
                    var name = faker.Address.Country();
                    var code = faker.Address.CountryCode()?.Trim().ToLowerInvariant() ?? "un";

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

                // Transaction guarantees all-or-nothing
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // 1) Add countries (no SaveChanges here)
                    await _countryRepository.BulkCreateAsync(countries, cancellationToken);

                    // 2) Save once so SQL Server generates Country.Id values
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                    // Now countries list has IDs filled in by EF Core.
                    var countryIds = countries.Select(c => c.Id).ToArray();

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

                    // 3) Add cities (no SaveChanges)
                    await _cityRepository.BulkCreateAsync(cities, cancellationToken);

                    // 4) Commit: SaveChanges + transaction commit (
                    // If your CommitTransactionAsync already calls SaveChanges, then do ONLY CommitTransactionAsync
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
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