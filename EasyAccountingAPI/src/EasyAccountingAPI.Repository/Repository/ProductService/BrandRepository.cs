namespace EasyAccountingAPI.Repository.Repository.ProductService
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public BrandRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext) 
        {
            _companyRepository = companyRepository;
        }

        public override async Task<Brand?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var brand = await db.Brands              
                .Where(b => b.Id == id && !b.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return brand;
        }

        // Get brand with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Brand>> GetBrandsByFilterAsync(FilterPageModel model, string? userId,
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = this._companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Brand, bool>> filter = b =>
                 !b.IsDeleted
                 && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(b.Id))
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || b.Name.Contains(model.FilterValue)
                 || b.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Brand, object>>>
            {
                ["name"] = c => c.Name,
                ["company"] = c => c.Company.Name,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, c => c.Id, sortableColumns, 
                include: q => q.Include(c => c.Company), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetBrandSelectList(string userId, CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await this._companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getBrands = db.Brands.AsNoTracking().Where(b => companyIds.Contains(b.Id) && !b.IsDeleted);

            return await getBrands
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel { Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);
        }
    }
}