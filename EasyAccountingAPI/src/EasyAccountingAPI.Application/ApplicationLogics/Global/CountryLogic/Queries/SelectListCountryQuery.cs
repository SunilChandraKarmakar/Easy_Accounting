namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CountryLogic.Queries
{
    public class SelectListCountryQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCountryQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICountryRepository _countryRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, ICountryRepository countryRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _countryRepository = countryRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCountryQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getCountries = await _countryRepository.GetCountrySelectList(cancellationToken);
                return getCountries;
            }
        }
    }
}