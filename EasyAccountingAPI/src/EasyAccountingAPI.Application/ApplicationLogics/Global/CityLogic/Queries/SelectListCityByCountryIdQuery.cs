namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Queries
{
    public class SelectListCityByCountryIdQuery : IRequest<IEnumerable<SelectModel>>
    {
        public int CountryId { get; set; }

        public class Handler : IRequestHandler<SelectListCityByCountryIdQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICityRepository _cityRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, ICityRepository cityRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _cityRepository = cityRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCityByCountryIdQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getCities = await _cityRepository.GetCityByCountryIdSelectList(request.CountryId, cancellationToken);
                return getCities;
            }
        }
    }
}