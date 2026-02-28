namespace EasyAccountingAPI.Repository.Contracts.Authentication
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<IEnumerable<SelectModel>> SelectListEmployeeByCompanyAsync(int companyId, CancellationToken cancellationToken);
        Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
        Task<FilterPageResultModel<Employee>> GetEmployeesByFilterAsync(FilterPageModel model,
            string? userId, CancellationToken cancellationToken);
    }
}