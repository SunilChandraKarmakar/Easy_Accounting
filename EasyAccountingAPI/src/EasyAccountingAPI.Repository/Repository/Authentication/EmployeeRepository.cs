namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<IEnumerable<SelectModel>> SelectListEmployeeByCompanyAsync(int companyId, 
            CancellationToken cancellationToken)
        {
            var employees = await db.Employees
                .AsNoTracking()
                .Where(e => e.CompanyId == companyId)
                .Select(e => new SelectModel
                {
                    Id = e.Id,
                    Name = e.FullName,
                })
                .ToListAsync(cancellationToken);

            return employees;
        }
    }
}