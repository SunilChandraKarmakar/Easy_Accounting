namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Queries
{
    public class GetCountriesQuery : IRequest<IEnumerable<CountryGridModel>>
    {
        public class Handler : IRequestHandler<GetCountriesQuery, IEnumerable<CountryGridModel>>
        {
            private readonly ICountryManager _countryManager;
            private readonly IMapper _mapper;

            public Handler(ICountryManager countryManager, IMapper mapper)
            {
                _countryManager = countryManager;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CountryGridModel>> Handle(GetCountriesQuery request, 
                CancellationToken cancellationToken)
            {
                // Get countries and map to grid model
                var getCountries = await _countryManager.GetAllAsync();
                var mapCountries = _mapper.Map<IEnumerable<CountryGridModel>>(getCountries);
                
                return mapCountries;
            }
        }
    }
}