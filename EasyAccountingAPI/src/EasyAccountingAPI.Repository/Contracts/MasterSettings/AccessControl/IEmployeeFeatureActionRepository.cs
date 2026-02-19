namespace EasyAccountingAPI.Repository.Contracts.MasterSettings.AccessControl
{
    public interface IEmployeeFeatureActionRepository : IBaseRepository<EmployeeFeatureAction>
    {
        Task<FilterPageResultModel<EmployeeFeatureAction>> GetEmployeeFeatureActionsByFilterAsync(
            FilterPageModel filterPageModel,
            CancellationToken cancellationToken);
        Task<bool> DeleteEmployeeFeatureActionByEmployeeAsync(int employeeId, CancellationToken cancellationToken);
    }
}