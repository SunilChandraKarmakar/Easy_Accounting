namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Command
{
    public class DeleteFeatureCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteFeatureCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IFeatureRepository _featureRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, IFeatureRepository featureRepository,
                IHttpContextAccessor httpContextAccessor)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _featureRepository = featureRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<bool> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the feature id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var featureId))
                    return false;

                // Fetch the feature
                var feature = await _featureRepository.GetByIdAsync(featureId, cancellationToken);
                if (feature is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    feature.IsDeleted = true;
                    feature.DeletedDateTime = DateTime.UtcNow;
                    _featureRepository.Update(feature);

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