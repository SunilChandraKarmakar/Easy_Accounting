namespace EasyAccountingAPI.Repository.Repository.MasterSettings.AccessControl
{
    public class FeatureActionRepository : BaseRepository<FeatureAction>, IFeatureActionRepository
    {
        public FeatureActionRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<FeatureAction>> GetAllAsync(CancellationToken cancellationToken)
        {
            var getFeatureActions = await db.FeatureActions
                .AsNoTracking()
                .Include(fa => fa.Feature)
                .Include(fa => fa.Action)
                .ToListAsync(cancellationToken);

            return getFeatureActions;
        }

        public override async Task<FeatureAction?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var featureAction = await db.FeatureActions
                .Where(fa => fa.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            return featureAction;
        }

        // Get features with filtering, sorting, and pagination
        public Task<FilterPageResultModel<FeatureAction>> GetFeatureActionsByFilterAsync(FilterPageModel model, 
            CancellationToken cancellationToken)
        {
            Expression<Func<FeatureAction, bool>> filter = f =>
                 string.IsNullOrWhiteSpace(model.FilterValue)
                 || f.Feature.Name.Contains(model.FilterValue)
                 || f.Action.Name.Contains(model.FilterValue);

            var sortableColumns = new Dictionary<string, Expression<Func<FeatureAction, object>>>
            {
                ["featureName"] = c => c.Feature.Name,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, f => f.Id, sortableColumns, 
                include: q => q.Include(c => c.Feature).Include(c => c.Action), cancellationToken);
        }

        public async Task<ICollection<FeatureAction>> GetFeatureActionsByFeatureIdAsync(int featureId, CancellationToken cancellationToken)
        {
            var featureActions = await db.FeatureActions
                .AsNoTracking()
                .Where(fa => fa.FeatureId == featureId)
                .ToListAsync(cancellationToken);

            return featureActions;
        }
    }
}