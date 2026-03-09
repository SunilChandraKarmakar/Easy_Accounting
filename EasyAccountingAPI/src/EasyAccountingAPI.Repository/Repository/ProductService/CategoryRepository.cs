namespace EasyAccountingAPI.Repository.Repository.ProductService
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public CategoryRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }

        public override async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var category = await db.Categories
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return category;
        }

        // Get category with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Category>> GetCategoriesByFilterAsync(FilterPageModel model, string? userId,
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Category, bool>> filter = c =>
                  !c.IsDeleted
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(c.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || c.Name.Contains(model.FilterValue)
                  || c.ParentCategory.Name.Contains(model.FilterValue)
                  || c.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Category, object>>>
            {
                ["name"] = b => b.Name,
                ["parent"] = b => b.ParentCategory.Name,
                ["company"] = b => b.Company.Name,
                ["id"] = b => b.Id
            };

            return GetAllFilterAsync(model, filter, c => c.Id, sortableColumns,
                include: q => q.Include(x => x.ParentCategory).Include(x => x.Company), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetCategorySelectList(string userId, 
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getCategories = db.Categories
                .AsNoTracking()
                .Where(c => companyIds.Contains(c.CompanyId) && !c.IsDeleted);

            return await getCategories
                .OrderBy(b => b.Name)
                .Select(s => new SelectModel { Id = s.Id, Name = s.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetParentCategorySelectList(string userId, 
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            // Get only parent categories (where ParentCategoryId is null)
            var getCategories = db.Categories
                .AsNoTracking()
                .Where(c => companyIds.Contains(c.CompanyId) && c.ParentId == null && !c.IsDeleted);

            return await getCategories
                .OrderBy(b => b.Name)
                .Select(s => new SelectModel { Id = s.Id, Name = s.Name })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetCategorySelectListByParentIdAsync(int parentId, string userId,
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getCategories = db.Categories
                .AsNoTracking()
                .Where(c => companyIds.Contains(c.CompanyId) && c.ParentId == parentId && !c.IsDeleted);

            return await getCategories
                .OrderBy(b => b.Name)
                .Select(s => new SelectModel { Id = s.Id, Name = s.Name })
                .ToListAsync(cancellationToken);
        }
    }
}