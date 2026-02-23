namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Queries
{
    public class GetEmloyeeFeatureActionsByFilterQuery : FilterPageModel,
        IRequest<FilterPageResultModel<EmployeeFeatureActionGridModel>>
    {
        public class Handler : IRequestHandler<GetEmloyeeFeatureActionsByFilterQuery, 
            FilterPageResultModel<EmployeeFeatureActionGridModel>>
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

            public async Task<FilterPageResultModel<EmployeeFeatureActionGridModel>> Handle(
                GetEmloyeeFeatureActionsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var result = 
                    await _employeeFeatureActionRepository.GetEmployeeFeatureActionsByFilterAsync(request, cancellationToken);

                var groupedData = result.Items
                    .GroupBy(x => new { x.EmployeeId, x.Employee.FullName })
                    .Select(employeeGroup => new EmployeeFeatureActionGridModel
                    {
                        EmployeeEncryptedId = EncryptionService.Encrypt(employeeGroup.Key.EmployeeId.ToString()),
                        EmployeeId = employeeGroup.Key.EmployeeId,
                        EmployeeName = employeeGroup.Key.FullName,

                        Features = employeeGroup
                            .GroupBy(f => new { f.FeatureId, f.Feature.Name })
                            .Select(featureGroup => new FeatureWithActionsModel
                            {
                                FeatureId = featureGroup.Key.FeatureId,
                                FeatureName = featureGroup.Key.Name,

                                Actions = featureGroup
                                    .Select(a => new ActionDetails
                                    {
                                        Id = a.ActionId,
                                        Name = a.Action.Name
                                    })
                                    .DistinctBy(a => a.Id)
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList();

                return new FilterPageResultModel<EmployeeFeatureActionGridModel>(groupedData, result.TotalCount);
            }
        }
    }
}