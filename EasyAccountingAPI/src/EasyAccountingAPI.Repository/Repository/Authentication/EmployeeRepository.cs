namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public EmployeeRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }

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
                    Group = e.EmployeeFeatureActions.Any() ? "Has Permission" : "No Permission",
                })
                .ToListAsync(cancellationToken);

            return employees;
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var employee = await db.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email == email, cancellationToken);

            return employee;
        }

        public override async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var employee = await db.Employees                
                .Where(e => e.Id == id && !e.IsDeleted)
                .Include(e => e.Company)
                .Include(e => e.EmployeeRoles)
                .FirstOrDefaultAsync();

            return employee;
        }

        public async Task<FilterPageResultModel<Employee>> GetEmployeesByFilterAsync(FilterPageModel model,
            string? userId, CancellationToken cancellationToken)
        {
            // Get company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken);

            Expression<Func<Employee, bool>> filter = vt =>
                 !vt.IsDeleted
                 && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains((int)vt.CompanyId!))
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || vt.FullName.Contains(model.FilterValue)
                 || vt.Phone.Contains(model.FilterValue)
                 || vt.Email.ToString().Contains(model.FilterValue)
                 || vt.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Employee, object>>>
            {
                ["name"] = c => c.FullName,
                ["phone"] = c => c.Phone,
                ["email"] = c => c.Email,
                ["company"] = c => c.Company.Name,
                ["id"] = c => c.Id
            };

            return await GetAllFilterAsync(model, filter, vt => vt.Id, sortableColumns,
                include: q => q.Include(x => x.Company), cancellationToken);
        }
    }
}