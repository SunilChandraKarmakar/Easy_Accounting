namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Command
{
    public class CreateActionSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateActionSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IActionRepository _actionRepository;

            public Handler(IUnitOfWorkRepository unitOfWork, IActionRepository actionRepository)
            {
                _unitOfWork = unitOfWork;
                _actionRepository = actionRepository;
            }

            public async Task<bool> Handle(CreateActionSeedCommand request, CancellationToken cancellationToken)
            {
                // Get the list of seed actions
                var seedActions = Actions();

                // Get all existing actions
                var existingActionNames = await _actionRepository.GetAllAsync(cancellationToken);

                // Create a HashSet of existing action names for efficient lookup
                var existingNamesSet = existingActionNames
                    .Select(a => a.Name.ToLower())
                    .ToHashSet();

                // Filter out actions that already exist and create new action entities for those that don't
                var newActions = seedActions
                    .Where(name => !existingNamesSet.Contains(name.ToLower()))
                    .Select(name => new EasyAccountingAPI.Model.MasterSettings.AccessControl.Action
                    {
                        Name = name
                    })
                    .ToList();

                if (newActions.Count == 0)
                    return false;

                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    await _actionRepository.BulkCreateAsync(newActions, cancellationToken);
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

            // Actions
            private static List<string> Actions()
            {
                var actions = new List<string>
                {
                    "Create",
                    "Update",
                    "Delete",
                    "List"
                };

                return actions;
            }
        }
    }
}