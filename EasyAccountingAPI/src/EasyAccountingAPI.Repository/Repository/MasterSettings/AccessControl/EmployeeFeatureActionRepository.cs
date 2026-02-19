namespace EasyAccountingAPI.Repository.Repository.MasterSettings.AccessControl
{
    public class EmployeeFeatureActionRepository : BaseRepository<EmployeeFeatureAction>, IEmployeeFeatureActionRepository
    {
        public EmployeeFeatureActionRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public Task<FilterPageResultModel<EmployeeFeatureAction>> GetEmployeeFeatureActionsByFilterAsync(
            FilterPageModel filterPageModel, CancellationToken cancellationToken)
        {
            Expression<Func<EmployeeFeatureAction, bool>> filter = x =>
                 (string.IsNullOrWhiteSpace(filterPageModel.FilterValue)
                 || x.Employee.FullName.Contains(filterPageModel.FilterValue)
                 || x.Feature.Name.Contains(filterPageModel.FilterValue)
                 || x.Action.Name.Contains(filterPageModel.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<EmployeeFeatureAction, object>>>
            {
                ["employeename"] = x => x.Employee.FullName,
                ["featurename"] = x => x.Feature.Name,
                ["actionname"] = x => x.Action.Name,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(filterPageModel, filter, c => c.Id, sortableColumns, 
                include: x => x.Include(x => x.Employee).Include(x => x.Feature).Include(x => x.Action), 
                cancellationToken);
        }

        public async Task<bool> DeleteEmployeeFeatureActionByEmployeeAsync(int employeeId, CancellationToken cancellationToken)
        {
            var affectedRows = await db.EmployeeFeatureActions
                .Where(efa => efa.EmployeeId == employeeId)
                .ExecuteDeleteAsync(cancellationToken);

            return affectedRows > 0;
        }
    }
}