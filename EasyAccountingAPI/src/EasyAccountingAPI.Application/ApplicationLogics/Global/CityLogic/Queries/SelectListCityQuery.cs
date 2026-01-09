namespace EasyAccountingAPI.Application.ApplicationLogics.Global.CityLogic.Queries
{
    public class SelectListCityQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCityQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICityRepository _cityRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, ICityRepository cityRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _cityRepository = cityRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCityQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getCities = await _cityRepository.GetCitySelectList(cancellationToken);
                return getCities;
            }
        }
    }
}