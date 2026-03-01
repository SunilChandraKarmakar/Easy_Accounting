namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EmployeeLogic.Command
{
    public class UpdateEmployeeCommand : EmployeeUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateEmployeeCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IEmployeeRepository _employeeRepository;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IEmployeeRepository employeeRepository,
                UserManager<User> userManager,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _employeeRepository = employeeRepository;
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
            {
                // Get logged-in user
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get employee with tracking
                var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

                if (employee is null)
                    return false;

                // Begin Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Update Employee fields
                    _mapper.Map(request, employee);

                    employee.UpdatedById = userId;
                    employee.UpdatedDateTime = DateTime.UtcNow;

                    _employeeRepository.Update(employee);

                    // Update Identity User (single query)
                    var user = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.EmployeeId == employee.Id, cancellationToken);

                    if (user != null)
                    {
                        user.FullName = request.FullName;
                        user.Email = request.Email;
                        user.UserName = request.Email;

                        var identityResult = await _userManager.UpdateAsync(user);
                        if (!identityResult.Succeeded)
                            throw new Exception("Failed to update user.");
                    }

                    // Save + Commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}