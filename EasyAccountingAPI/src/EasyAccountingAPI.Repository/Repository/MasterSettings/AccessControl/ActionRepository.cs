namespace EasyAccountingAPI.Repository.Repository.MasterSettings.AccessControl
{
    public class ActionRepository : BaseRepository<Model.MasterSettings.AccessControl.Action>, IActionRepository
    {
        public ActionRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<Model.MasterSettings.AccessControl.Action>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await db.Actions
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Model.MasterSettings.AccessControl.Action?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var action = await db.Actions
                .Where(a => a.Id == id && !a.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return action;
        }

        // Get actions with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Model.MasterSettings.AccessControl.Action>> GetActionsByFilterAsync(FilterPageModel model, 
            CancellationToken cancellationToken)
        {
            Expression<Func<Model.MasterSettings.AccessControl.Action, bool>> filter = a =>
                 !a.IsDeleted
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || a.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Model.MasterSettings.AccessControl.Action, object>>>
            {
                ["name"] = c => c.Name,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, c => c.Id, sortableColumns, include: null, cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetActionSelectList(CancellationToken cancellationToken)
        {
            var getActions = db.Actions.AsNoTracking().Where(c => !c.IsDeleted);

            return await getActions
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel { Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return await db.Actions
                .AsNoTracking()
                .AnyAsync(c => !c.IsDeleted, cancellationToken);
        }

        public async Task<bool> IsExistActionByNameAsync(string actionName, CancellationToken cancellationToken)
        {
            var hasAction = await db.Actions
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.Name.ToLower() == actionName.ToLower())
                .FirstOrDefaultAsync(cancellationToken);

            return hasAction is not null ? true : false;
        }
    }
}