namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.Command
{
    public class DeleteActionCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteActionCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IActionRepository _actionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, IActionRepository actionRepository, 
                IHttpContextAccessor httpContextAccessor)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _actionRepository = actionRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<bool> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the action id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var actionId))
                    return false;

                // Fetch the action
                var action = await _actionRepository.GetByIdAsync(actionId, cancellationToken);
                if (action is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    action.IsDeleted = true;
                    action.DeletedDateTime = DateTime.UtcNow;
                    _actionRepository.Update(action);

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