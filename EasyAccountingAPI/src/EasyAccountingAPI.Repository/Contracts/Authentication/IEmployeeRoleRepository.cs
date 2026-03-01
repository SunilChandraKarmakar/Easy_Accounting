namespace EasyAccountingAPI.Repository.Contracts.Authentication
{
    public interface IEmployeeRoleRepository : IBaseRepository<EmployeeRole>
    {
        Task<string> GetEmployeeRoleNameByEmployeeId(int employeeId);
        Task<EmployeeRole?> GetEmployeeRoleByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
    }
}