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
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Brand, bool>> filter = b =>
                  !b.IsDeleted
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(b.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || b.Name.Contains(model.FilterValue)
                  || b.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Brand, object>>>
            {
                ["name"] = b => b.Name,
                ["company"] = b => b.Company.Name,
                ["id"] = b => b.Id
            };

            return GetAllFilterAsync(model, filter, b => b.Id, sortableColumns, 
                include: q => q.Include(x => x.Company), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetBrandSelectList(string userId, CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getBrands = db.Brands.AsNoTracking().Where(b => companyIds.Contains(b.Id) && !b.IsDeleted);

            return await getBrands
                .OrderBy(b => b.Name)
                .Select(s => new SelectModel { Id = s.Id, Name = s.Name })
                .ToListAsync(cancellationToken);
        }
    }
}