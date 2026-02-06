namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Command
{
    public class DeleteFeatureActionCommand : IRequest<bool>
    {
        public int FeatureId { get; set; }

        public class Handler : IRequestHandler<DeleteFeatureActionCommand, bool>
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

            public async Task<bool> Handle(DeleteFeatureActionCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch the feature action based on the feature id
                var featureActions = await _featureActionRepository.GetFeatureActionsByFeatureIdAsync(request.FeatureId, cancellationToken);
                if (featureActions is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    await _featureActionRepository.BulkDeleteAsync(featureActions, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}