namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Queries
{
    public class SelectListFeatureByModuleIdQuery : IRequest<IEnumerable<SelectModel>>
    {
        public int ModuleId { get; set; }

        public class Handler : IRequestHandler<SelectListFeatureByModuleIdQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IFeatureRepository _featureRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IFeatureRepository featureRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _featureRepository = featureRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListFeatureByModuleIdQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                if(request.ModuleId <= 0)
                    return Enumerable.Empty<SelectModel>();

                var getFeatures = await _featureRepository.GetFeatureSelectListByModule(request.ModuleId, cancellationToken);
                return getFeatures;
            }
        }
    }
}