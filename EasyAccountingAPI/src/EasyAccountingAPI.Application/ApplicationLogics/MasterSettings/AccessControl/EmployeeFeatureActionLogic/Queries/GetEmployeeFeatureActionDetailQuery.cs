namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Queries
{
    public class GetEmployeeFeatureActionDetailQuery : IRequest<ICollection<EmployeeFeatureActionUpdateModel>>
    {
        public string EmployeeId { get; set; }

        public class Handler : IRequestHandler<GetEmployeeFeatureActionDetailQuery, ICollection<EmployeeFeatureActionUpdateModel>>
        {
            private readonly IEmployeeFeatureActionRepository _employeeFeatureActionRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(
                IEmployeeFeatureActionRepository employeeFeatureActionRepository, 
                IHttpContextAccessor httpContextAccessor,
                IMapper mapper)
            {
                _employeeFeatureActionRepository = employeeFeatureActionRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<ICollection<EmployeeFeatureActionUpdateModel>> Handle(GetEmployeeFeatureActionDetailQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                if (string.IsNullOrEmpty(request.EmployeeId) || string.IsNullOrWhiteSpace(request.EmployeeId) 
                    || request.EmployeeId == "-1")
                    return new List<EmployeeFeatureActionUpdateModel>();

                // Decrypt the employee id
                var decryptedId = EncryptionService.Decrypt(request.EmployeeId);
                if (!int.TryParse(decryptedId, out var employeeId))
                    return new List<EmployeeFeatureActionUpdateModel>();

                // Get employee feature actions by employee id
                var employeeFeatureActions = await _employeeFeatureActionRepository.GetEmployeeFeatureActionsByEmployeeIdAsync  (employeeId, cancellationToken);

                // Map to update model
                var mappedEmployeeFeatureActions = _mapper.Map<ICollection<EmployeeFeatureActionUpdateModel>>(employeeFeatureActions);

                return mappedEmployeeFeatureActions;
            }
        }
    }
}