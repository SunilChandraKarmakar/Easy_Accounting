namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Queries
{
    public class GetEmployeeFeatureActionDetailQuery : IRequest<EmployeeFeatureActionUpdateModel>
    {
        public string EmployeeId { get; set; }

        public class Handler : IRequestHandler<GetEmployeeFeatureActionDetailQuery, EmployeeFeatureActionUpdateModel>
        {
            private readonly IEmployeeFeatureActionRepository _employeeFeatureActionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(
                IEmployeeFeatureActionRepository employeeFeatureActionRepository, 
                IHttpContextAccessor httpContextAccessor)
            {
                _employeeFeatureActionRepository = employeeFeatureActionRepository;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<EmployeeFeatureActionUpdateModel> Handle(GetEmployeeFeatureActionDetailQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                if (string.IsNullOrEmpty(request.EmployeeId) || string.IsNullOrWhiteSpace(request.EmployeeId) 
                    || request.EmployeeId == "-1")
                    return new EmployeeFeatureActionUpdateModel();

                // Decrypt the employee id
                var decryptedId = EncryptionService.Decrypt(request.EmployeeId);
                if (!int.TryParse(decryptedId, out var employeeId))
                    return new EmployeeFeatureActionUpdateModel();

                return new EmployeeFeatureActionUpdateModel();
            }
        }
    }
}