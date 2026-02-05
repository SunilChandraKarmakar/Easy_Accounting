namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Queries
{
    public class GetFeatureActionsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<FeatureActionGridModel>>
    {
        public class Handler : IRequestHandler<GetFeatureActionsByFilterQuery, FilterPageResultModel<FeatureActionGridModel>>
        {
            private readonly IFeatureActionRepository _featureActionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(IFeatureActionRepository featureActionRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _featureActionRepository = featureActionRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<FeatureActionGridModel>> Handle(GetFeatureActionsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get the feature IDs that match the filter criteria for the user
                var featureIds = await _featureActionRepository.GetPagedFeatureIdsAsync(request, cancellationToken);

                if (!featureIds.Any())
                    return new FilterPageResultModel<FeatureActionGridModel>(new List<FeatureActionGridModel>(), 0);

                // Get actions for those features
                var rawData = await _featureActionRepository.GetFeatureActionsByFilterAsync(new FilterPageModel
                {
                    PageIndex = 0,
                    PageSize = int.MaxValue,
                    FilterValue = request.FilterValue
                },
                cancellationToken);

                // Group data
                var grouped = rawData.Items
                    .Where(x => featureIds.Contains(x.FeatureId))
                    .GroupBy(x => new { x.FeatureId, x.Feature.Name })
                    .Select(g => new FeatureActionGridModel
                    {
                        FeatureId = g.Key.FeatureId,
                        FeatureName = g.Key.Name,
                        Actions = g
                            .GroupBy(a => new { a.ActionId, a.Action.Name })
                            .Select(a => new FeatureActionStatusModel
                            {
                                ActionId = a.Key.ActionId,
                                ActionName = a.Key.Name,
                                // IsEnabled = a.Any(x => x.IsEnabled)
                            })
                            .ToList()
                    })
                    .ToList();

                // Get total feature count 
                var totalFeatures = await _featureActionRepository.GetTotalFeatureCountAsync(request, cancellationToken);
                return new FilterPageResultModel<FeatureActionGridModel>(grouped, totalFeatures);
            }
        }
    }
}