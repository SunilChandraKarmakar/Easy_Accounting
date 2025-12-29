namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Queries
{
    public class SelectListCityQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCityQuery, IEnumerable<SelectModel>>
        {
            private readonly ICityRepository _cityRepository;

            public Handler(ICityRepository cityRepository)
            {
                _cityRepository = cityRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCityQuery request, CancellationToken cancellationToken)
            {
                var getCities = await _cityRepository.GetCitySelectList(cancellationToken);
                return getCities;
            }
        }
    }
}