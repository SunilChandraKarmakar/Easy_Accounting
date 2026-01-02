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
                // Initialize identity result
                IdentityResult identityResult = new IdentityResult();

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

                    // Create new role for this user
                    var role = new Role
                    {
                        Name = "Admin",
                        Description = "Full access to manage your business operations and system settings.",
                        CreatedByEmployee = newEmployee
                    };

                    await _roleRepository.CreateAsync(role, cancellationToken);

                    // Create new employee role mapping
                    var newEmployeeRole = new EmployeeRole
                    {
                        Employee = newEmployee,
                        Role = role,
                        AssignedAt = DateTime.UtcNow,
                        AssignedByEmployee = newEmployee
                    };

                    await _employeeRoleRepository.CreateAsync(newEmployeeRole, cancellationToken);

                    // Create new user
                    var registerUser = _mapper.Map<User>(request);
                    registerUser.UserName = request.Email;
                    registerUser.Email = request.Email;
                    registerUser.FullName = request.FullName;
                    registerUser.Employee = newEmployee;

                    identityResult = await _userManager.CreateAsync(registerUser, request.Password);             

                    if (identityResult.Succeeded)
                    {
                        await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                        await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                        var registerCompleteUser = _mapper.Map<UserModel>(registerUser);
                        return registerCompleteUser;
                    }                        
                    else
                        throw new Exception(identityResult.Errors.Select(s => s.Description).FirstOrDefault());
                }
                catch (Exception)
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw new Exception(identityResult.Errors.Select(s => s.Description).FirstOrDefault());
                }
            }
        }
    }
}