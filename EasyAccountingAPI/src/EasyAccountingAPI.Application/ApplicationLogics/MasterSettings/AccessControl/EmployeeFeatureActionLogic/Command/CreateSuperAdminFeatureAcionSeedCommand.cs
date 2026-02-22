namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.EmployeeFeatureActionLogic.Command
{
    public class CreateSuperAdminFeatureAcionSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateSuperAdminFeatureAcionSeedCommand, bool>
        {
            private readonly IEmployeeRepository _employeeRepository;
            private readonly IEmployeeFeatureActionRepository _employeeFeatureActionRepository;
            private readonly IActionRepository _actionRepository;
            private readonly IFeatureRepository _featureRepository;
            private readonly IUnitOfWorkRepository _unitOfWork;

            public Handler(
                IEmployeeRepository employeeRepository,
                IEmployeeFeatureActionRepository employeeFeatureActionRepository,
                IActionRepository actionRepository,
                IFeatureRepository featureRepository,
                IUnitOfWorkRepository unitOfWork)
            {
                _employeeRepository = employeeRepository;
                _employeeFeatureActionRepository = employeeFeatureActionRepository;
                _actionRepository = actionRepository;
                _featureRepository = featureRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(CreateSuperAdminFeatureAcionSeedCommand request, CancellationToken cancellationToken)
            {
                var superAdminEmail = "super_admin@gmail.com";
                var superAdminEmployee = await _employeeRepository.GetEmployeeByEmailAsync(superAdminEmail, cancellationToken);

                if (superAdminEmployee is null)
                    return false;

                // Get all features and actions
                var features = await _featureRepository.GetAllAsync(cancellationToken);
                var actions = await _actionRepository.GetAllAsync(cancellationToken);

                // Get all existing feature-actions for super admin in ONE query
                var existingFeatureActions =
                    await _employeeFeatureActionRepository
                    .GetEmployeeFeatureActionsByEmployeeIdAsync(superAdminEmployee.Id, cancellationToken);

                // Create HashSet for fast lookup (O(1))
                var existingSet = existingFeatureActions
                    .Select(x => (x.FeatureId, x.ActionId))
                    .ToHashSet();

                var newFeatureActions = new List<EmployeeFeatureAction>();

                foreach (var feature in features)
                {
                    foreach (var action in actions)
                    {
                        if (!existingSet.Contains((feature.Id, action.Id)))
                        {
                            newFeatureActions.Add(new EmployeeFeatureAction
                            {
                                EmployeeId = superAdminEmployee.Id,
                                FeatureId = feature.Id,
                                ActionId = action.Id
                            });
                        }
                    }
                }

                if (!newFeatureActions.Any())
                    return true;

                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    await _employeeFeatureActionRepository.BulkCreateAsync(newFeatureActions, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}