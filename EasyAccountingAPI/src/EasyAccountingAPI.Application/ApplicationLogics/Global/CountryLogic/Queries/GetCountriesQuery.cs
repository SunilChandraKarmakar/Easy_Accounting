namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Queries
{
    public class GetCountriesQuery : IRequest<ICollection<CountryGridModel>>
    {
        public class Handler : IRequestHandler<GetCountriesQuery, ICollection<CountryGridModel>>
        {
            private readonly ICountryRepository _countryRepository;
            private readonly IMapper _mapper;

            public Handler(ICountryRepository countryepository, IMapper mapper)
            {
                _countryRepository = countryepository;
                _mapper = mapper;
            }

            public async Task<ICollection<CountryGridModel>> Handle(GetCountriesQuery request, 
                CancellationToken cancellationToken)
            {
                // Get countries and map to grid model
                var getCountries = await _countryRepository.GetAllAsync();
                var mapCountries = _mapper.Map<ICollection<CountryGridModel>>(getCountries);

                return mapCountries;
            }
        }
    }
}