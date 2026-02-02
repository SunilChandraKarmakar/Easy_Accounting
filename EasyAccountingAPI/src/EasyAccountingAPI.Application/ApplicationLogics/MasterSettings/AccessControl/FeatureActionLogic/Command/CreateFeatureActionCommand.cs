namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Command
{
    public class CreateFeatureActionCommand : FeatureActionCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateFeatureActionCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IFeatureActionRepository _featureActionRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IUnitOfWorkRepository unitOfWorkRepository,
                IFeatureActionRepository featureActionRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _featureActionRepository = featureActionRepository;
            }

            public async Task<bool> Handle(CreateFeatureActionCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Create list feature actions
                var createdFeatureActions = new List<FeatureAction>();

                if (request.ActionIds == null || !request.ActionIds.Any())
                    return false;

                foreach (var actionId in request.ActionIds)
                {
                    var featureAction = new FeatureAction
                    {
                        FeatureId = request.FeatureId,
                        ActionId = actionId
                    };

                    createdFeatureActions.Add(featureAction);
                }

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Add range feature actions
                    await _featureActionRepository.BulkCreateAsync(createdFeatureActions, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}