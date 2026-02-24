namespace EasyAccountingAPI.Repository.Repository.MasterSettings.AccessControl
{
    public class EmployeeFeatureActionRepository : BaseRepository<EmployeeFeatureAction>, IEmployeeFeatureActionRepository
    {
        public EmployeeFeatureActionRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<FilterPageResultModel<EmployeeFeatureAction>> GetEmployeeFeatureActionsByFilterAsync(
            FilterPageModel filterPageModel, CancellationToken cancellationToken)
        {
            // Base query with includes
            IQueryable<EmployeeFeatureAction> query = db.Set<EmployeeFeatureAction>()
                .AsNoTracking()
                .Include(x => x.Employee)
                .Include(x => x.Feature)
                .Include(x => x.Action);

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterPageModel.FilterValue))
            {
                string filterValue = filterPageModel.FilterValue;
                query = query.Where(x =>
                    x.Employee.FullName.Contains(filterValue) ||
                    x.Feature.Name.Contains(filterValue) ||
                    x.Action.Name.Contains(filterValue));
            }

            // Get distinct employee IDs after filtering
            var employeeQuery = query
                .Select(x => new EmployeeProjection
                {
                    EmployeeId = x.EmployeeId,
                    FullName = x.Employee.FullName
                })
                .Distinct();

            // Define sortable columns for employee pagination
            var employeeSortableColumns = new Dictionary<string, Expression<Func<EmployeeProjection, object>>>
            {
                ["employeename"] = x => x.FullName,
                ["id"] = x => x.EmployeeId
            };

            // Apply sorting based on requested column
            if (!string.IsNullOrWhiteSpace(filterPageModel.SortColumn)
                && employeeSortableColumns.TryGetValue(filterPageModel.SortColumn.ToLower(), out var sortExpression))
            {
                employeeQuery = filterPageModel.SortOrder?.ToLower() == "descend"
                    ? employeeQuery.OrderByDescending(sortExpression)
                    : employeeQuery.OrderBy(sortExpression);
            }
            else
            {
                employeeQuery = employeeQuery.OrderBy(x => x.EmployeeId);
            }

            // Count total employees
            int totalCount = await employeeQuery.CountAsync(cancellationToken);

            // Apply pagination on employees
            var pagedEmployeeIds = await employeeQuery
                .Skip(filterPageModel.PageIndex * filterPageModel.PageSize)
                .Take(filterPageModel.PageSize)
                .Select(x => x.EmployeeId)
                .ToListAsync(cancellationToken);

            // Get all feature actions for paged employees
            var items = await query
                .Where(x => pagedEmployeeIds.Contains(x.EmployeeId))
                .ToListAsync(cancellationToken);

            return new FilterPageResultModel<EmployeeFeatureAction>(items, totalCount);
        }

        public async Task<EmployeeFeatureAction?> GetEmployeeFeatureActionByEmployeeAndFeatureAndActionAsync(int employeeId,
            int featureId, int actionId, CancellationToken cancellationToken)
        {
            var employeeFeatureAction = await db.EmployeeFeatureActions
                .FirstOrDefaultAsync(efa => efa.EmployeeId == employeeId && efa.FeatureId == featureId
                    && efa.ActionId == actionId, cancellationToken);

            return employeeFeatureAction;
        }

        public async Task<bool> DeleteEmployeeFeatureActionByEmployeeAsync(int employeeId, CancellationToken cancellationToken)
        {
            var affectedRows = await db.EmployeeFeatureActions
                .Where(efa => efa.EmployeeId == employeeId)
                .ExecuteDeleteAsync(cancellationToken);

            return affectedRows > 0;
        }

        public async Task<IEnumerable<EmployeeFeatureAction>> GetEmployeeFeatureActionsByEmployeeIdAsync(int employeeId, 
            CancellationToken cancellationToken)
        {
            var employeeFeatureActions = await db.EmployeeFeatureActions
                .AsNoTracking()
                .Where(efa => efa.EmployeeId == employeeId)
                .Include(efa => efa.Employee)
                    .ThenInclude(e => e.Company)
                .Include(efa => efa.Feature)
                    .ThenInclude(f => f.Module)
                .ToListAsync(cancellationToken);

            return employeeFeatureActions;
        }
    }

    public class EmployeeProjection
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}