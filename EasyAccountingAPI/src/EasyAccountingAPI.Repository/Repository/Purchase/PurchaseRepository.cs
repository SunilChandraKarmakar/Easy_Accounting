namespace EasyAccountingAPI.Repository.Repository.Purchase
{
    public class PurchaseRepository : BaseRepository<Model.Purchase.Purchase>, IPurchaseRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public PurchaseRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }

        public override async Task<Model.Purchase.Purchase?> GetByIdAsync(int id, CancellationToken ct)
        {
            var purchase = await db.Purchase
                .Where(p => p.Id == id && !p.IsDeleted)
                .Include(p => p.PurchaseItems)
                .FirstOrDefaultAsync(ct);

            return purchase;
        }

        // Get purchase with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Model.Purchase.Purchase>> GetPurchasesByFilterAsync(FilterPageModel model,
            string? userId, CancellationToken ct)
        {
            // Get employee based company ids
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, ct).Result;

            Expression<Func<Model.Purchase.Purchase, bool>> filter = p =>
                  !p.IsDeleted
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(p.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || p.Company.Name.Contains(model.FilterValue)
                  || p.PurchaseDate.ToString().Contains(model.FilterValue)
                  || p.PaymentDate.ToString().Contains(model.FilterValue)
                  || p.Vendor.FullName.Contains(model.FilterValue)
                  || p.Notes.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Model.Purchase.Purchase, object>>>
            {
                ["company"] = p => p.Company.Name,
                ["vendor"] = p => p.Vendor.FullName,
                ["id"] = p => p.Id
            };

            return GetAllFilterAsync(model, filter, p => p.Id, sortableColumns,
                include: q => q.Include(p => p.Company).Include(p => p.Vendor), ct);
        }

        public async Task<IEnumerable<SelectModel>> GetPurchaseSelectList(string userId, CancellationToken ct)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, ct);

            var purchases = db.Purchase
                .AsNoTracking()
                .Where(p => companyIds.Contains(p.CompanyId) && !p.IsDeleted);

            return await purchases
                .OrderBy(p => p.OrderNumber)
                .Select(s => new SelectModel { Id = s.Id, Name = s.OrderNumber })
                .ToListAsync(ct);
        }
    }
}