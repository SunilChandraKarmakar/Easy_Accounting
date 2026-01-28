namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.Command
{
    public class UpdateActionCommand : ActionUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateActionCommand, bool>
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

            public async Task<bool> Handle(UpdateActionCommand request, CancellationToken ct)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing action
                var getExistingAction = await _actionRepository.GetByIdAsync(request.Id, ct);
                if (getExistingAction is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(ct);

                try
                {
                    _mapper.Map((ActionUpdateModel)request, getExistingAction);
                    _actionRepository.Update(getExistingAction);
                    await _unitOfWorkRepository.SaveChangesAsync(ct);
                    await _unitOfWorkRepository.CommitTransactionAsync(ct);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(ct);
                    return false;
                }
            }
        }
    }
}