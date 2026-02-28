namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EmployeeLogic.Queries
{
    public class GetEmployeeByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<EmployeeGridModel>>
    {
        public class Handler : IRequestHandler<GetEmployeeByFilterQuery, FilterPageResultModel<EmployeeGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IEmployeeRepository _employeeRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IEmployeeRepository employeeRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _employeeRepository = employeeRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<EmployeeGridModel>> Handle(GetEmployeeByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get employees and map to grid model
                var getEmployees = await _employeeRepository.GetEmployeesByFilterAsync(request, userId, cancellationToken);

                // Map employees
                var mapEmployees = _mapper.Map<ICollection<EmployeeGridModel>>(getEmployees.Items);

                // Return paginated result
                return new FilterPageResultModel<EmployeeGridModel>(mapEmployees, getEmployees.TotalCount);
            }
        }
    }
}