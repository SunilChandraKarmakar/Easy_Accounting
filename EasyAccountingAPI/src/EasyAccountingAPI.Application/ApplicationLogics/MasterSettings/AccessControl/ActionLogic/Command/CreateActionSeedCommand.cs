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
                // Prevent duplicate seeding
                if (await _actionRepository.AnyAsync(cancellationToken))
                    return true;

                // Seed default actions
                var actions = GetDefaultActions();

                // Transactional operation
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    await _actionRepository.BulkCreateAsync(actions, cancellationToken);
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

            // Action list
            private static List<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action> GetDefaultActions()
            {
                return new List<EasyAccountingAPI.Model.MasterSettings.AccessControl.Action>
                {
                    new() { Name = "Create" },
                    new() { Name = "Update" },
                    new() { Name = "Delete" },
                    new() { Name = "List" },
                }
                .Select(c => new EasyAccountingAPI.Model.MasterSettings.AccessControl.Action
                {
                    Name = c.Name.Trim(),
                    IsDeleted = false,
                    DeletedDateTime = null
                })
                .ToList();
            }
        }
    }
}