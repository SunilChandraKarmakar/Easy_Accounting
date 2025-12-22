namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Command
{
    public class CreateCountryCommand : CountryCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCountryCommand, bool>
        {
            private readonly ICountryManager _countryManager;
            private readonly ICityManager _cityManager;
            private readonly IMapper _mapper;

            public Handler(ICountryManager countryManager, ICityManager cityManager, IMapper mapper)
            {
                _countryManager = countryManager;
                _cityManager = cityManager;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
            {
                var createdCountry = _mapper.Map<Country>(request);
                createdCountry = await _countryManager.CreateAsync(createdCountry);

                // Check, cities is not null and has items
                if(request.Cities is not null && request.Cities.Count > 0)
                {
                    // Prepare cities
                    var prepiredCities = new List<City>();

                    foreach (var city in request.Cities)
                    {
                        var cityEntity = _mapper.Map<City>(city);
                        cityEntity.CountryId = createdCountry.Id;

                        // Add to prepared list
                        prepiredCities.Add(cityEntity);
                    }

                    // Bulk insert cities
                    if(prepiredCities is not null && prepiredCities.Count > 0)
                        await _cityManager.BulkCreateAsync(prepiredCities);
                }

                if (createdCountry is not null && createdCountry.Id > 0)
                    return true;

                return false;
            }
        }
    }
}