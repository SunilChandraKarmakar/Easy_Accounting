namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.Queries
{
    public class SelectListActionQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListActionQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IActionRepository _actionRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IActionRepository actionRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _actionRepository = actionRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListActionQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getActions = await _actionRepository.GetActionSelectList(cancellationToken);
                return getActions;
            }
        }
    }
}