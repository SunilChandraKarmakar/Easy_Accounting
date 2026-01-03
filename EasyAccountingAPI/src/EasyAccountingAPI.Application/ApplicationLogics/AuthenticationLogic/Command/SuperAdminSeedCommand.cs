namespace EasyAccountingAPI.Application.ApplicationLogics.AuthenticationLogic.Command
{
    public class SuperAdminSeedCommand : RegisterModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<SuperAdminSeedCommand, bool>
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

            public async Task<bool> Handle(SuperAdminSeedCommand request, CancellationToken cancellationToken)
            {
                // Super admin seeding email & password
                // email: super_admin@gmail.com
                // password: Super_Admin@123#

                // Initialize identity result
                IdentityResult identityResult = new IdentityResult();

                // Email uniqueness check
                if (await _userManager.FindByEmailAsync("super_admin@gmail.com") != null)
                    return false;

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create new employee for this user
                    var newEmployee = new Employee
                    {
                        FullName = "Super Admin",
                        Phone = "+8801743909840",
                        Email = "super_admin@gmail.com",
                        Image = string.Empty
                    };

                    await _employeeRepository.CreateAsync(newEmployee, cancellationToken);

                    // Create new role for this user
                    var role = new Role
                    {
                        Name = "Super Admin",
                        Description = "Full access to Easy Accounting for manage operations and system settings.",
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
                    registerUser.UserName = "super_admin@gmail.com";
                    registerUser.Email = "super_admin@gmail.com";
                    registerUser.FullName = "Super Admin";
                    registerUser.Employee = newEmployee;

                    identityResult = await _userManager.CreateAsync(registerUser, "Super_Admin@123#");

                    if (identityResult.Succeeded)
                    {
                        await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                        await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception)
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}