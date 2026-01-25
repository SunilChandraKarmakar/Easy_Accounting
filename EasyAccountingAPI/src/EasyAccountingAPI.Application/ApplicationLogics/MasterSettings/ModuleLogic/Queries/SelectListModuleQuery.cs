namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Queries
{
    public class SelectListModuleQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListModuleQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IModuleRepository _moduleRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IModuleRepository moduleRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _moduleRepository = moduleRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListModuleQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getModules = await _moduleRepository.GetModuleSelectList(cancellationToken);
                return getModules;
            }
        }
    }
}