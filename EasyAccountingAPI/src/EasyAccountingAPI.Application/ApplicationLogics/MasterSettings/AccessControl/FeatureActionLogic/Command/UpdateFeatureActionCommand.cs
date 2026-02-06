namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Command
{
    public class UpdateFeatureActionCommand : FeatureActionUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateFeatureActionCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IFeatureActionRepository _featureActionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, IFeatureActionRepository featureActionRepository,
                IHttpContextAccessor httpContextAccessor)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _featureActionRepository = featureActionRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<bool> Handle(UpdateFeatureActionCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check, if at last one action is selected
                if(request.ActionIds.Count == 0) return false;

                // Fetch feature action based on feature id
                var getFeatureActions = await _featureActionRepository.GetFeatureActionsByFeatureIdAsync(request.FeatureId, cancellationToken);
                if (getFeatureActions is null) return false;

                // Create new feature actions based on selected action ids
                var createdFeatureActions = new List<FeatureAction>();
                foreach (var actionId in request.ActionIds)
                {
                    var featureAction = new FeatureAction
                    {
                        FeatureId = request.FeatureId,
                        ActionId = actionId
                    };

                    createdFeatureActions.Add(featureAction);
                }

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    await _featureActionRepository.BulkDeleteAsync(getFeatureActions, cancellationToken);
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