namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Queries
{
    public class SelectListFeatureQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListFeatureQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IFeatureRepository _featureRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IFeatureRepository featureRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _featureRepository = featureRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListFeatureQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getFeatures = await _featureRepository.GetFeatureSelectList(cancellationToken);
                return getFeatures;
            }
        }
    }
}