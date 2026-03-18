namespace EasyAccountingAPI.Repository.Repository.ProductService
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public ProductRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository)
            : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }

        public override async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductInventories)
                .Where(p => p.Id == id && !p.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return product;
        }

        // Get product with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Product>> GetProductsByFilterAsync(FilterPageModel model, string? userId,
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Product, bool>> filter = p =>
                  !p.IsDeleted
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(p.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || p.Name.Contains(model.FilterValue)
                  || p.Code.Contains(model.FilterValue)
                  || p.ProductUnit.Name.Contains(model.FilterValue)
                  || p.Category.Name.Contains(model.FilterValue)
                  || p.Brand.Name.Contains(model.FilterValue)
                  || p.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Product, object>>>
            {
                ["name"] = p => p.Name,
                ["code"] = p => p.Code,
                ["unit"] = p => p.ProductUnit.Name,
                ["category"] = p => p.Category.Name,
                ["brand"] = p => p.Brand.Name,
                ["company"] = p => p.Company.Name,
                ["id"] = p => p.Id
            };

            return GetAllFilterAsync(model, filter, b => b.Id, sortableColumns,
                include: q => q.Include(x => x.ProductUnit)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Company), cancellationToken);
        }

        // Get expired product with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Product>> GetExpiredProductsByFilterAsync(FilterPageModel model, 
            string? userId, CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Product, bool>> filter = p =>
                  !p.IsDeleted
                  && (p.ProductInventories.Any(pi => pi.ExpiryDate < DateTime.Now))
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(p.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || p.Name.Contains(model.FilterValue)
                  || p.Code.Contains(model.FilterValue)
                  || p.ProductUnit.Name.Contains(model.FilterValue)
                  || p.Category.Name.Contains(model.FilterValue)
                  || p.Brand.Name.Contains(model.FilterValue)
                  || p.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Product, object>>>
            {
                ["name"] = p => p.Name,
                ["code"] = p => p.Code,
                ["unit"] = p => p.ProductUnit.Name,
                ["category"] = p => p.Category.Name,
                ["brand"] = p => p.Brand.Name,
                ["company"] = p => p.Company.Name,
                ["id"] = p => p.Id
            };

            return GetAllFilterAsync(model, filter, b => b.Id, sortableColumns,
                 include: q => q.Include(p => p.ProductInventories)
                .Include(x => x.ProductUnit)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Company), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetProductSelectList(string userId, 
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getProducts = db.Products.AsNoTracking().Where(b => companyIds.Contains(b.CompanyId) && !b.IsDeleted);

            return await getProducts
                .OrderBy(b => b.Name)
                .Select(s => new SelectModel { Id = s.Id, Name = s.Name })
                .ToListAsync(cancellationToken);
        }
    }
}