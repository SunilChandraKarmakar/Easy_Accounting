namespace EasyAccountingAPI.Application.ApplicationLogics.AuthenticationLogic.Command
{
    public class RegistrationCommand : RegisterModel, IRequest<UserModel>
    {
        public class Handler : IRequestHandler<RegistrationCommand, UserModel>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly UserManager<User> _userManager;
            private readonly IEmployeeRepository _employeeRepository;
            private readonly IRoleRepository _roleRepository;
            private readonly IEmployeeRoleRepository _employeeRoleRepository;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, UserManager<User> userManager,
                IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IEmployeeRoleRepository employeeRoleRepository,
                IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _userManager = userManager;
                _employeeRepository = employeeRepository;
                _roleRepository = roleRepository;
                _employeeRoleRepository = employeeRoleRepository;
                _mapper = mapper;
            }

            public async Task<UserModel> Handle(RegistrationCommand request, CancellationToken cancellationToken)
            {
                // Email uniqueness check
                if (await _userManager.FindByEmailAsync(request.Email) != null)
                    throw new Exception("This email address is already registered. Please use a different email address.");

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create new employee for this user
                    var newEmployee = new Employee
                    {
                        FullName = request.FullName,
                        Phone = request.Phone,
                        Email = request.Email,
                        Image = string.Empty
                    };

                    await _employeeRepository.CreateAsync(newEmployee, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                    // Create new role for this user
                    var role = new Role
                    {
                        Name = "Admin",
                        Description = "Full access to manage your business operations and system settings.",
                        CreatedByEmployeeId = newEmployee.Id
                    };

                    await _roleRepository.CreateAsync(role, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                    // Create new employee role mapping
                    var newEmployeeRole = new EmployeeRole
                    {
                        EmployeeId = newEmployee.Id,
                        RoleId = role.Id,
                        AssignedAt = DateTime.UtcNow,
                        AssignedByEmployeeId = newEmployee.Id
                    };

                    await _employeeRoleRepository.CreateAsync(newEmployeeRole, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                    // Create new user
                    var registerUser = _mapper.Map<User>(request);
                    registerUser.UserName = request.Email;
                    registerUser.Email = request.Email;
                    registerUser.FullName = request.FullName;
                    registerUser.EmployeeId = newEmployee.Id;

                    var result = _userManager.CreateAsync(registerUser, request.Password);
                    var registerCompleteUser = _mapper.Map<UserModel>(registerUser);

                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    if (result.Result.Succeeded)
                        return registerCompleteUser;
                    else
                        throw new Exception(result.Result.Errors.Select(s => s.Description).FirstOrDefault());
                }
                catch (Exception)
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw new Exception("The registration process could not be completed due to a system issue. Please try again later.");
                }
            }
        }
    }
}