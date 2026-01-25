namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<Module?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var module = await db.Modules
                .Where(m => m.Id == id && !m.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return module;
        }

        // Get module with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Module>> GetModulesByFilterAsync(FilterPageModel model, CancellationToken cancellationToken)
        {
            Expression<Func<Module, bool>> filter = c =>
                 !c.IsDeleted
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || c.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Module, object>>>
            {
                ["name"] = c => c.Name,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, c => c.Id, sortableColumns, include: null, cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetModuleSelectList(CancellationToken cancellationToken)
        {
            var getModules = db.Modules.AsNoTracking().Where(c => !c.IsDeleted);

            return await getModules
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel { Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return await db.Modules
                .AsNoTracking()
                .AnyAsync(m => !m.IsDeleted, cancellationToken);
        }
    }
}