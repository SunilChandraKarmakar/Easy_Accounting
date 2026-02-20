namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EmployeeLogic.Queries
{
    public class SelectListEmployeeByCompanyQuery : IRequest<IEnumerable<SelectModel>>
    {
        public int CompanyId { get; set; }

        public class Handler : IRequestHandler<SelectListEmployeeByCompanyQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IEmployeeRepository _employeeRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IEmployeeRepository employeeRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _employeeRepository = employeeRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListEmployeeByCompanyQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getModules = await _employeeRepository.SelectListEmployeeByCompanyAsync(request.CompanyId, cancellationToken);
                return getModules;
            }
        }
    }
}