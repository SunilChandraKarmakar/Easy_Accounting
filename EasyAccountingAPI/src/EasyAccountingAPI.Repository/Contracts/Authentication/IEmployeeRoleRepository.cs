namespace EasyAccountingAPI.Repository.Contracts.Authentication
{
    public interface IEmployeeRoleRepository : IBaseRepository<EmployeeRole>
    {
        Task<string> GetEmployeeRoleNameByEmployeeId(int employeeId);
    }
}