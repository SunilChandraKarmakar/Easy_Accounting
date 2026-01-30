namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<Company?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var company = await db.Companies
                .Include(c => c.Country)
                .Include(c => c.City)
                .Include(c => c.Currency)
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return company;
        }

        // Get companies with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Company>> GetCompaniesByFilterAsync(FilterPageModel model, string? userId,
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
    }
}