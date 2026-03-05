namespace EasyAccountingAPI.Repository.Repository.ProductService
{
    public class VariationRepository : BaseRepository<Variation>, IVariationRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public VariationRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }

        public override async Task<Variation?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var variation = await db.Variations
                .Where(v => v.Id == id && !v.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return variation;
        }

        // Get variation with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Variation>> GetVariationsByFilterAsync(FilterPageModel model, string? userId,
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Variation, bool>> filter = v =>
                  !v.IsDeleted
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(v.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || v.Name.Contains(model.FilterValue)
                  || v.Values.Contains(model.FilterValue)
                  || v.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Variation, object>>>
            {
                ["name"] = v => v.Name,
                ["value"] = v => v.Values,
                ["company"] = v => v.Company.Name,
                ["id"] = v => v.Id
            };

            return GetAllFilterAsync(model, filter, v => v.Id, sortableColumns,
                include: q => q.Include(x => x.Company), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetVariationSelectList(string userId, CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getVariations = db.Variations.AsNoTracking().Where(c => companyIds.Contains(c.CompanyId) && !c.IsDeleted);

            return await getVariations
                .OrderBy(b => b.Name)
                .Select(s => new SelectModel { Id = s.Id, Name = s.Name })
                .ToListAsync(cancellationToken);
        }
    }
}