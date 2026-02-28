namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EmployeeLogic.Queries
{
    public class GetEmployeeDetailQuery : IRequest<EmployeeUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetEmployeeDetailQuery, EmployeeUpdateModel>
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

            public async Task<EmployeeUpdateModel> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                if (request.Id == "-1")
                    return new EmployeeUpdateModel();

                // Decrypt the employee id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var employeeId))
                    return new EmployeeUpdateModel();

                // Get employee by id
                var getEmployee = await _employeeRepository.GetByIdAsync(employeeId, cancellationToken);

                if (getEmployee is null)
                    return new EmployeeUpdateModel();

                // Map employee
                var mapEmployee = _mapper.Map<EmployeeUpdateModel>(getEmployee);
                return mapEmployee;
            }
        }
    }
}