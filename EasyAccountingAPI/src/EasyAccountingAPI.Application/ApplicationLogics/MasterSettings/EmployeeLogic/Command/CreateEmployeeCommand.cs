namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EmployeeLogic.Command
{
    public class CreateEmployeeCommand : EmployeeCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateEmployeeCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IEmployeeRepository _employeeRepository;
            private readonly UserManager<User> _userManager;
            private readonly IRoleRepository _roleRepository;
            private readonly IEmployeeRoleRepository _employeeRoleRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IEmployeeRepository employeeRepository,
                UserManager<User> userManager,
                IRoleRepository roleRepository,
                IEmployeeRoleRepository employeeRoleRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _employeeRepository = employeeRepository;
                _userManager = userManager;
                _roleRepository = roleRepository;
                _employeeRoleRepository = employeeRoleRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get login user employee id
                var getLoginUser = await _userManager.FindByIdAsync(userId);

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create employee
                    var createdEmployee = _mapper.Map<Employee>(request);
                    createdEmployee.CreatedById = userId;
                    createdEmployee.CreatedDateTime = DateTime.UtcNow;
                    await _employeeRepository.CreateAsync(createdEmployee, cancellationToken);

                    // Get employee role
                    var role = await _roleRepository.GetRoleByNameAsync("Employee", cancellationToken);

                    // Set employee role
                    var employeeRole = new EmployeeRole
                    {
                        Employee = createdEmployee,
                        Role = role,
                        AssignedAt = DateTime.UtcNow,
                        AssignedByEmployeeId = (int)getLoginUser!.EmployeeId!
                    };
                    await _employeeRoleRepository.CreateAsync(employeeRole, cancellationToken);

                    // Create user for employee
                    var registerUser = new User();
                    registerUser.UserName = request.Email;
                    registerUser.Email = request.Email;
                    registerUser.FullName = request.FullName;
                    registerUser.Employee = createdEmployee;
                    await _userManager.CreateAsync(registerUser, request.Password);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return createdEmployee.Id > 0;
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