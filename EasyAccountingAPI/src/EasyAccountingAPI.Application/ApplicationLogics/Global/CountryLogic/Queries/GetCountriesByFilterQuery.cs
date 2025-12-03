namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Queries
{
    public class GetCountriesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<CountryGridModel>>
    {
        public class Handler : IRequestHandler<GetCountriesByFilterQuery, FilterPageResultModel<CountryGridModel>>
        {
            private readonly ICountryManager _countryManager;
            private readonly IMapper _mapper;

            public Handler(ICountryManager countryManager, IMapper mapper)
            {
                _countryManager = countryManager;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<CountryGridModel>> Handle(GetCountriesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Get countries and map to grid model
                var getCountries = await _countryManager.GetCountriesByFilterAsync(request);
                var mapCountries = _mapper.Map<ICollection<CountryGridModel>>(getCountries.Items);

                // Return paginated result
                return new FilterPageResultModel<CountryGridModel>(mapCountries, getCountries.TotalCount);
            }
        }
    }
}