namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // For file or image validation
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB

        public CompanyRepository(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment) : base(databaseContext) 
        {
            _webHostEnvironment = webHostEnvironment;
        }

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
            // Get employee based company ids
            var companyIds = GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Company, bool>> filter = c =>
                 !c.IsDeleted
                 && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(c.Id))
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

        public async Task<IEnumerable<SelectModel>> GetCompanySelectList(IHttpContextAccessor httpContextAccessor,
           string userId, CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getCompaines = db.Companies.AsNoTracking().Where(c => companyIds.Contains(c.Id) && !c.IsDeleted);     

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
        public async Task<List<int>> GetEmployeeBasedCompanyIdsAsync(string userId, CancellationToken cancellationToken)
        {
            var employeeId = await db.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.EmployeeId)
                .FirstOrDefaultAsync(cancellationToken);

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
                    .ToListAsync(cancellationToken);
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
                    .ToListAsync(cancellationToken);
            }

            // Super Admin
            return await db.Companies
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<string?> SaveCompanyLogoAsync(IFormFile? file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("Logo file size must not exceed 2 MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
                throw new InvalidOperationException("Only .jpg, .jpeg, .png, and .webp files are allowed.");

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "upload_images", "company");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var safeFileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(uploadsFolder, safeFileName);

            await using var stream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return $"/upload_images/company/{safeFileName}";
        }

        public void DeleteLogoFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            var cleanedPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, cleanedPath);

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);
        }
    }
}