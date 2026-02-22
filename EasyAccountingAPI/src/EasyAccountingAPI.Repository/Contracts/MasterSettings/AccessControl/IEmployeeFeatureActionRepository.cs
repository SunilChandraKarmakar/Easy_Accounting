namespace EasyAccountingAPI.Repository.Contracts.MasterSettings.AccessControl
{
    public interface IEmployeeFeatureActionRepository : IBaseRepository<EmployeeFeatureAction>
    {
        Task<FilterPageResultModel<EmployeeFeatureAction>> GetEmployeeFeatureActionsByFilterAsync(FilterPageModel filterPageModel,
            CancellationToken cancellationToken);
        Task<bool> DeleteEmployeeFeatureActionByEmployeeAsync(int employeeId, CancellationToken cancellationToken);
        Task<EmployeeFeatureAction?> GetEmployeeFeatureActionByEmployeeAndFeatureAndActionAsync(int employeeId,
            int featureId, int actionId, CancellationToken cancellationToken);
        Task<IEnumerable<EmployeeFeatureAction>> GetEmployeeFeatureActionsByEmployeeIdAsync(int employeeId, 
            CancellationToken cancellationToken);
    }
}