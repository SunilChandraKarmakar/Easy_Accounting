namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.Command
{
    public class CreateActionCommand : ActionCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateActionCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IActionRepository _actionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, IActionRepository actionRepository,
                IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _actionRepository = actionRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateActionCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create action
                    var action = _mapper.Map<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action>(request);
                    await _actionRepository.CreateAsync(action, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return action.Id > 0;
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