namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Command
{
    public class UpdateEmployeeFeatureActionCommand : List<EmployeeFeatureActionUpdateModel>, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateEmployeeFeatureActionCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IEmployeeFeatureActionRepository _employeeFeatureActionRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IEmployeeFeatureActionRepository employeeFeatureActionRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _employeeFeatureActionRepository = employeeFeatureActionRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateEmployeeFeatureActionCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check, if the request is null or empty, then return false
                if (request is null || !request.Any() || request.Any(x => x.EmployeeId <= 0))
                    return false;

                // Get employee id
                var employeeId = request.First().EmployeeId;

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Delete existing EmployeeFeatureAction by employee id
                    await _employeeFeatureActionRepository.DeleteEmployeeFeatureActionByEmployeeAsync(employeeId, cancellationToken);

                    // Newly create EmployeeFeatureAction
                    var employeeFeatureActions = _mapper.Map<List<EmployeeFeatureAction>>(request);
                    await _employeeFeatureActionRepository.BulkCreateAsync(employeeFeatureActions, cancellationToken);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
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