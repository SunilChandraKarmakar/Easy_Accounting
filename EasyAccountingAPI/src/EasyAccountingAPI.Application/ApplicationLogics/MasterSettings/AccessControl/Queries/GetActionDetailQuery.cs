namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.Queries
{
    public class GetActionDetailQuery : IRequest<ActionUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetActionDetailQuery, ActionUpdateModel>
        {
            private readonly IActionRepository _actionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(IActionRepository actionRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _actionRepository = actionRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<ActionUpdateModel> Handle(GetActionDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the action id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var actionId))
                    return new ActionUpdateModel();

                // Get action by id
                var getAction = await _actionRepository.GetByIdAsync(actionId, cancellationToken);

                if (getAction is null)
                    return new ActionUpdateModel();

                // Map action
                var mapAction = _mapper.Map<ActionUpdateModel>(getAction);
                return mapAction;
            }
        }
    }
}