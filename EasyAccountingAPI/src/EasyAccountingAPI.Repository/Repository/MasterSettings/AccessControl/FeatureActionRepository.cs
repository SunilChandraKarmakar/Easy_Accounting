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

        public async Task<List<FeatureAction>> GetFeatureActionsByFeatureIdsAsync(List<int> featureIds, CancellationToken cancellationToken)
        {
            return await db.FeatureActions
                .AsNoTracking()
                .Include(x => x.Feature)
                .Include(x => x.Action)
                .Where(x => featureIds.Contains(x.FeatureId))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<int>> GetPagedFeatureIdsAsync(FilterPageModel model, CancellationToken cancellationToken)
        {
            return await db.FeatureActions
                .AsNoTracking()
                .Where(x => string.IsNullOrWhiteSpace(model.FilterValue)
                    || x.Feature.Name.Contains(model.FilterValue)
                    || x.Action.Name.Contains(model.FilterValue))
                .Select(x => new
                {
                    x.FeatureId,
                    FeatureName = x.Feature.Name
                })
                .Distinct()
                .OrderBy(x => x.FeatureName) 
                .Skip(model.PageIndex * model.PageSize)
                .Take(model.PageSize)
                .Select(x => x.FeatureId)
                .ToListAsync(cancellationToken);
        }


        public async Task<int> GetTotalFeatureCountAsync(FilterPageModel model, CancellationToken cancellationToken)
        {
            return await db.FeatureActions
                .AsNoTracking()
                .Where(x =>
                    string.IsNullOrWhiteSpace(model.FilterValue)
                    || x.Feature.Name.Contains(model.FilterValue)
                    || x.Action.Name.Contains(model.FilterValue))
                .Select(x => x.FeatureId)
                .Distinct()
                .CountAsync(cancellationToken);
        }

        public async Task<ICollection<FeatureAction>> GetFeatureActionsByFeatureIdAsync(int featureId, CancellationToken cancellationToken)
        {
            var featureActions = await db.FeatureActions
                .Where(fa => fa.FeatureId == featureId)
                .Include(fa => fa.Feature)
                .ToListAsync(cancellationToken);

            return featureActions;
        }
    }
}