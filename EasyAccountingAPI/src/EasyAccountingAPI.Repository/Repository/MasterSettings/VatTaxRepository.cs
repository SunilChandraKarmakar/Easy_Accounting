namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class VatTaxRepository : BaseRepository<VatTax>, IVatTaxRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public VatTaxRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext) 
        {
            _companyRepository = companyRepository;
        }

        public override async Task<VatTax?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var vatTax = await db.VatTaxes
                .Where(vt => vt.Id == id && !vt.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return vatTax;
        }

        // Get vat tax with filtering, sorting, and pagination
        public async Task<FilterPageResultModel<VatTax>> GetVatTaxesByFilterAsync(FilterPageModel model, 
            string? userId, CancellationToken cancellationToken)
        {
            // Get company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken);

            Expression<Func<VatTax, bool>> filter = vt =>
                 !vt.IsDeleted
                 && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(vt.CompanyId))
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || vt.TaxName.Contains(model.FilterValue)
                 || vt.TaxNumber.Contains(model.FilterValue)
                 || vt.Rate.ToString().Contains(model.FilterValue)
                 || vt.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<VatTax, object>>>
            {
                ["name"] = c => c.TaxName,
                ["number"] = c => c.TaxNumber,
                ["rate"] = c => c.Rate,
                ["company"] = c => c.Company.Name,
                ["id"] = c => c.Id
            };

            return await GetAllFilterAsync(model, filter, vt => vt.Id, sortableColumns,
                include: q => q.Include(x => x.Company), cancellationToken);
        }
    }
}