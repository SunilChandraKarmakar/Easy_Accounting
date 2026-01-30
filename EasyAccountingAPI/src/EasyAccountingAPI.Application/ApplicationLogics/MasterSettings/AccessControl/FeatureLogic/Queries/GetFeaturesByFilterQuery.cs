namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Queries
{
    public class GetFeaturesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<FeatureGridModel>>
    {
        public class Handler : IRequestHandler<GetFeaturesByFilterQuery, FilterPageResultModel<FeatureGridModel>>
        {
            private readonly IFeatureRepository _feaureRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(IFeatureRepository featureRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _feaureRepository = featureRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<FeatureGridModel>> Handle(GetFeaturesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get features and map to grid model
                var getFeatures = await _feaureRepository.GetFeaturesByFilterAsync(request, cancellationToken);
                var mapFeatures = _mapper.Map<ICollection<FeatureGridModel>>(getFeatures.Items);

                // Return paginated result
                return new FilterPageResultModel<FeatureGridModel>(mapFeatures, getFeatures.TotalCount);
            }
        }
    }
}