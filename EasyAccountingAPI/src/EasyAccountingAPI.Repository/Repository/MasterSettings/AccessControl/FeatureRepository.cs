namespace EasyAccountingAPI.Repository.Repository.MasterSettings.AccessControl
{
    public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
    {
        public FeatureRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<Feature>> GetAllAsync(CancellationToken cancellationToken)
        {
            var getFeatures = await db.Features
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .ToListAsync(cancellationToken);

            return getFeatures;
        }

        public override async Task<Feature?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var feature = await db.Features
                .Include(f => f.Module)
                .Where(f => f.Id == id && !f.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return feature;
        }

        // Get features with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Feature>> GetFeaturesByFilterAsync(FilterPageModel model, CancellationToken cancellationToken)
        {
            Expression<Func<Feature, bool>> filter = f =>
                 !f.IsDeleted
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || f.Name.Contains(model.FilterValue)
                 || f.Code.Contains(model.FilterValue)
                 || f.Module.Name.Contains(model.FilterValue)
                 || f.ControllerName!.Contains(model.FilterValue)
                 || f.TableName!.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Feature, object>>>
            {
                ["name"] = c => c.Name,
                ["moduleName"] = c => c.Module.Name,
                ["controllerName"] = c => c.ControllerName!,
                ["tableName"] = c => c.TableName!,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, f => f.Id, sortableColumns, include: q => q.Include(c => c.Module), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetFeatureSelectList(CancellationToken cancellationToken)
        {
            var getFeatures = db.Features.AsNoTracking().Where(f => !f.IsDeleted);

            return await getFeatures
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel { Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetFeatureSelectListByModule(int moduleId, CancellationToken cancellationToken)
        {
            var getFeatures = db.Features.AsNoTracking().Where(f => f.ModuleId == moduleId && !f.IsDeleted);

            return await getFeatures
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel { Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<Feature?> GetFeatureByTableNameAsync(string tableName, CancellationToken cancellationToken)
        {
            var feature = await db.Features
                .AsNoTracking()
                .Where(f => f.TableName!.ToLower() == tableName.ToLower() && !f.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return feature;
        }
    }
}