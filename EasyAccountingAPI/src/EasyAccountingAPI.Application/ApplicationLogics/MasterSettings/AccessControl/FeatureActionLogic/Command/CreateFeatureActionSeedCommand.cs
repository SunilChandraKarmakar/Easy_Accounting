namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureActionLogic.Command
{
    public class CreateFeatureActionSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateFeatureActionSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IActionRepository _actionRepository;
            private readonly IFeatureRepository _featureRepository;
            private readonly IFeatureActionRepository _featureActionRepository;

            public Handler(
                IUnitOfWorkRepository unitOfWork,
                IActionRepository actionRepository,
                IFeatureRepository featureRepository,
                IFeatureActionRepository featureActionRepository)
            {
                _unitOfWork = unitOfWork;
                _actionRepository = actionRepository;
                _featureRepository = featureRepository;
                _featureActionRepository = featureActionRepository;
            }

            public async Task<bool> Handle(CreateFeatureActionSeedCommand request, CancellationToken cancellationToken)
            {
                var actions = await _actionRepository.GetAllAsync(cancellationToken);
                var features = await _featureRepository.GetAllAsync(cancellationToken);
                var existingFeatureActions = await _featureActionRepository.GetAllAsync(cancellationToken);

                var existingSet = new HashSet<(int FeatureId, int ActionId)>(existingFeatureActions.Select(x => (x.FeatureId, x.ActionId)));

                var newFeatureActions = new List<FeatureAction>();

                foreach (var feature in features)
                {
                    foreach (var action in actions)
                    {
                        var key = (feature.Id, action.Id);

                        if (!existingSet.Contains(key))
                        {
                            newFeatureActions.Add(new FeatureAction
                            {
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
                    await _featureActionRepository.BulkCreateAsync(newFeatureActions, cancellationToken);
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