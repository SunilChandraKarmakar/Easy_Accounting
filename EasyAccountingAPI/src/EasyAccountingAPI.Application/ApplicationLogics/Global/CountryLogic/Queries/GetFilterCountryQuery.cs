namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Queries
{
    public class GetFilterCountryQuery : IRequest<FilterPagedResult<CountryGridModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class Handler : IRequestHandler<GetFilterCountryQuery, FilterPagedResult<CountryGridModel>>
        {
            private readonly ICountryManager _countryManager;
            private readonly IMapper _mapper;

            public Handler(ICountryManager countryManager, IMapper mapper)
            {
                _countryManager = countryManager;
                _mapper = mapper;
            }

            public async Task<FilterPagedResult<CountryGridModel>> Handle(GetFilterCountryQuery request,
                CancellationToken cancellationToken)
            {
                // Get countries and map to grid model
                var getCountries = await _countryManager.GetCountriesFilterAsync(request.PageNumber, request.PageSize);
                var mapCountries = _mapper.Map<ICollection<CountryGridModel>>(getCountries.Items);

                return new FilterPagedResult<CountryGridModel>
                    (mapCountries, getCountries.TotalCount, request.PageNumber, request.PageSize);
            }
        }
    }
}