namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Queries
{
    public class GetFeatureActionsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<FeatureActionGridModel>>
    {
        public class Handler : IRequestHandler<GetFeatureActionsByFilterQuery, FilterPageResultModel<FeatureActionGridModel>>
        {
            private readonly IFeatureActionRepository _featureActionRepository;
            private readonly IActionRepository _actionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IFeatureActionRepository featureActionRepository, IActionRepository actionRepository, 
                IHttpContextAccessor httpContextAccessor)
            {
                _featureActionRepository = featureActionRepository;
                _actionRepository = actionRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<FilterPageResultModel<FeatureActionGridModel>> Handle(GetFeatureActionsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get paged feature ids based on filter and pagination
                var featureIds = await _featureActionRepository.GetPagedFeatureIdsAsync(request, cancellationToken);

                if (!featureIds.Any())
                    return new FilterPageResultModel<FeatureActionGridModel>(new List<FeatureActionGridModel>(), 0);

                // Get actions
                var allActions = await _actionRepository.GetAllAsync(cancellationToken);

                // Get feature actions based on featureIds
                var featureActions = await _featureActionRepository.GetFeatureActionsByFeatureIdsAsync(featureIds, cancellationToken);

                // Build a lookup of enabled (FeatureId, ActionId) pairs for O(1) checks
                var enabledLookup = new HashSet<(int FeatureId, int ActionId)>(featureActions.Select(fa => (fa.FeatureId, fa.ActionId)));

                // Map grid
                var result = featureActions
                    .GroupBy(fa => new { fa.FeatureId, fa.Feature.Name })
                    .Select(fg => new FeatureActionGridModel
                    {
                        FeatureId = fg.Key.FeatureId,
                        EncriptedFeatureId = EncryptionService.Encrypt(fg.Key.FeatureId.ToString()),
                        FeatureName = fg.Key.Name,
                        Actions = allActions.Select(a => new FeatureActionStatusModel
                        {
                            ActionId = a.Id,
                            ActionName = a.Name,
                            IsEnabled = enabledLookup.Contains((fg.Key.FeatureId, a.Id))
                        }).ToList()
                    })
                    .OrderBy(x => x.FeatureName)
                    .ToList();

                // Total feature count
                var totalCount = await _featureActionRepository.GetTotalFeatureCountAsync(request, cancellationToken);

                return new FilterPageResultModel<FeatureActionGridModel>(result, totalCount);
            }
        }
    }
}