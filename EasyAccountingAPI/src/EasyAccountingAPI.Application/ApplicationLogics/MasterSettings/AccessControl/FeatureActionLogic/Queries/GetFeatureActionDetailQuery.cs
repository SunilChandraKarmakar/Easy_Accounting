namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Queries
{
    public class GetFeatureActionDetailQuery : IRequest<FeatureActionUpdateModel>
    {
        public string FeatureId { get; set; }

        public class Handler : IRequestHandler<GetFeatureActionDetailQuery, FeatureActionUpdateModel>
        {
            private readonly IFeatureActionRepository _featureActionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IFeatureActionRepository featureActionRepository, IHttpContextAccessor httpContextAccessor)
            {
                _featureActionRepository = featureActionRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<FeatureActionUpdateModel> Handle(GetFeatureActionDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                if(string.IsNullOrEmpty(request.FeatureId) || string.IsNullOrWhiteSpace(request.FeatureId) || request.FeatureId == "-1")
                    return new FeatureActionUpdateModel();

                // Decrypt the feature id
                var decryptedId = EncryptionService.Decrypt(request.FeatureId);
                if (!int.TryParse(decryptedId, out var featureId))
                    return new FeatureActionUpdateModel();

                // Get feature actions by feature id
                var getFeatureActions = await _featureActionRepository.GetFeatureActionsByFeatureIdAsync(featureId, cancellationToken);

                if (getFeatureActions is null || getFeatureActions.Count == 0)
                    return new FeatureActionUpdateModel();

                // Get only feature id and action ids
                var getFeatureId = getFeatureActions?.FirstOrDefault()?.FeatureId;
                var getActionIds = getFeatureActions?.Select(fa => fa.ActionId).ToList();

                // Map feature actions
                var mapFeatureAction = new FeatureActionUpdateModel
                {
                    FeatureId = (int)getFeatureId!,
                    ActionIds = getActionIds!
                };

                return mapFeatureAction;
            }
        }
    }
}