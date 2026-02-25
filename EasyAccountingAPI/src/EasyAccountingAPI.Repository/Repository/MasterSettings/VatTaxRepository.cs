namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class VatTaxRepository : BaseRepository<VatTax>, IVatTaxRepository
    {
        public VatTaxRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<VatTax?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var vatTax = await db.VatTaxes
                .Where(vt => vt.Id == id && !vt.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return vatTax;
        }

        // Get vat tax with filtering, sorting, and pagination
        public Task<FilterPageResultModel<VatTax>> GetVatTaxesByFilterAsync(FilterPageModel model, string? userId,
            CancellationToken cancellationToken)
        {
            Expression<Func<Company, bool>> filter = c =>
                 !c.IsDeleted
                 && (string.IsNullOrWhiteSpace(userId) || c.CreatedById == userId)
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || c.Name.Contains(model.FilterValue)
                 || c.Email.Contains(model.FilterValue)
                 || c.Phone.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Company, object>>>
            {
                ["name"] = c => c.Name,
                ["email"] = c => c.Email,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, c => c.Id, sortableColumns,
                include: q => q.Include(c => c.Country).Include(c => c.City).Include(c => c.Currency), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetCompanySelectList(IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
        {
            // Retrieve the login user's info from the current HTTP context
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;

            var getCompaines = db.Companies.AsNoTracking().Where(c => !c.IsDeleted);

            if (userEmail is not "super_admin@gmail.com")
                getCompaines = getCompaines.Where(c => c.CreatedById == userId);

            return await getCompaines
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel { Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);
        }

        // Check, company created user has default company or not
        public async Task<bool> IsCreatedUserHaveDefaultCompany(string userId, CancellationToken cancellationToken)
        {
            return await db.Companies
                .AsNoTracking()
                .AnyAsync(c => !c.IsDeleted && c.IsDefaultCompany && c.CreatedById == userId, cancellationToken);
        }

        // Check, if create user select default company then remove old default company
        public async Task<bool> IsRemoveOldDefaultCompanyOfCreatedUser(string userId, CancellationToken cancellationToken)
        {
            var oldDefaultCOmpany = await db.Companies
                .Where(c => c.CreatedById == userId && c.IsDefaultCompany && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (oldDefaultCOmpany is not null)
            {
                oldDefaultCOmpany.IsDefaultCompany = false;
                db.Companies.Update(oldDefaultCOmpany);
                return true;
            }

            return false;
        }

        // Get login user's default company
        public async Task<Company> GetLoginUserDefaultCompany(string userId, CancellationToken cancellationToken)
        {
            var defaultCompany = await db.Companies
                .Include(c => c.Country)
                .Include(c => c.City)
                .Include(c => c.Currency)
                .Where(c => c.CreatedById == userId && c.IsDefaultCompany && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return defaultCompany!;
        }

        // Get employee based company ids
        public async Task<int[]> GetEmployeeBasedCompanyIdsAsync(string userId, CancellationToken cancellationToken)
        {
            // Get EmployeeId 
            var employeeId = await db.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.EmployeeId)
                .FirstOrDefaultAsync(cancellationToken);

            // Get role name 
            var roleName = await db.EmployeeRoles
                .AsNoTracking()
                .Where(er => er.EmployeeId == employeeId)
                .Select(er => er.Role.Name)
                .FirstOrDefaultAsync(cancellationToken);

            // Admin
            if (roleName == "Admin")
            {
                return await db.Companies
                    .AsNoTracking()
                    .Where(c => c.CreatedById == userId && !c.IsDeleted)
                    .Select(c => c.Id)
                    .ToArrayAsync(cancellationToken);
            }

            // Employee
            if (roleName == "Employee")
            {
                var adminCreatorId = await db.Employees
                    .AsNoTracking()
                    .Where(e => e.Id == employeeId && !e.IsDeleted)
                    .Select(e => e.CreatedById)
                    .FirstOrDefaultAsync(cancellationToken);

                return await db.Companies
                    .AsNoTracking()
                    .Where(c => c.CreatedById == adminCreatorId && !c.IsDeleted)
                    .Select(c => c.Id)
                    .ToArrayAsync(cancellationToken);
            }

            // Super Admin
            return await db.Companies
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Select(c => c.Id)
                .ToArrayAsync(cancellationToken);
        }
    }
}