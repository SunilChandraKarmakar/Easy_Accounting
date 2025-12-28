namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Queries
{
    public class SelectListCountryQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCountryQuery, IEnumerable<SelectModel>>
        {
            private readonly ICountryRepository _countryRepository;

            public Handler(ICountryRepository countryRepository)
            {
                _countryRepository = countryRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCountryQuery request, CancellationToken cancellationToken)
            {
                var getCountries = await _countryRepository.GetCountrySelectList(cancellationToken);
                return getCountries;
            }
        }
    }
}