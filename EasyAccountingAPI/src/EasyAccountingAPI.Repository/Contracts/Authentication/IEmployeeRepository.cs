namespace EasyAccountingAPI.Repository.Contracts.Authentication
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<IEnumerable<SelectModel>> SelectListEmployeeByCompanyAsync(int companyId, CancellationToken cancellationToken);
    }
}