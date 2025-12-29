namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Queries
{
    public class GetCityByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<CityGridModel>>
    {
        public class Handler : IRequestHandler<GetCityByFilterQuery, FilterPageResultModel<CityGridModel>>
        {
            private readonly ICityRepository _cityRepository;
            private readonly IMapper _mapper;

            public Handler(ICityRepository cityRepository, IMapper mapper)
            {
                _cityRepository = cityRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<CityGridModel>> Handle(GetCityByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Get cities and map to grid model
                var getCities = await _cityRepository.GetCitiesByFilterAsync(request, cancellationToken);
                var mapCities = _mapper.Map<ICollection<CityGridModel>>(getCities.Items);

                // Return paginated result
                return new FilterPageResultModel<CityGridModel>(mapCities, getCities.TotalCount);
            }
        }
    }
}