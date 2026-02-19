namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Command
{
    public class DeleteEmployeeFeatureActionCommand : IRequest<bool>
    {
        public int EmployeeId { get; set; }

        public class Handler : IRequestHandler<DeleteEmployeeFeatureActionCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IEmployeeFeatureActionRepository _employeeFeatureActionRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor, 
                IEmployeeFeatureActionRepository employeeFeatureActionRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _employeeFeatureActionRepository = employeeFeatureActionRepository;
            }

            public async Task<bool> Handle(DeleteEmployeeFeatureActionCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Delete the employee feature actions associated with the specified employee Id
                var isDeleted = 
                    await _employeeFeatureActionRepository.DeleteEmployeeFeatureActionByEmployeeAsync(request.EmployeeId,
                    cancellationToken);

                return isDeleted;
            }
        }
    }
}