namespace EasyAccountingAPI.Repository.Repository.MasterSettings.VendorInformation
{
    public class VendorRepository : BaseRepository<Vendor>, IVendorRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public VendorRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) 
            : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }

        public override async Task<Vendor?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var vendor = await db.Vendors
                .Include(v => v.VendorAddresses)
                .Where(v => v.Id == id && !v.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return vendor;
        }

        // Get vendor with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Vendor>> GetVendorsByFilterAsync(FilterPageModel model, string? userId,
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<Vendor, bool>> filter = v =>
                  !v.IsDeleted
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(v.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || v.BusinessName.Contains(model.FilterValue)
                  || v.FullName.Contains(model.FilterValue)
                  || v.Email.Contains(model.FilterValue)
                  || v.Phone.Contains(model.FilterValue)
                  || v.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Vendor, object>>>
            {
                ["business"] = b => b.BusinessName,
                ["name"] = b => b.FullName,
                ["email"] = b => b.Email,
                ["phone"] = b => b.Phone,
                ["company"] = b => b.Company.Name,
                ["id"] = b => b.Id
            };

            return GetAllFilterAsync(model, filter, b => b.Id, sortableColumns,
                include: q => q.Include(x => x.Company)
                .Include(v => v.VendorAddresses)
                    .ThenInclude(a => a.City)
                .Include(v => v.VendorAddresses)
                    .ThenInclude(a => a.Country)
                .AsSplitQuery(), 
                cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetVendorSelectList(string userId,
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);
            var getVendors = db.Vendors.AsNoTracking().Where(v => companyIds.Contains(v.CompanyId) && !v.IsDeleted);

            return await getVendors
                .OrderBy(b => b.FullName)
                .Select(s => new SelectModel { Id = s.Id, Name = s.FullName })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetVendorsByCompanyIdAsync(int companyId, CancellationToken ct)
        {
            var vendors = db.Vendors
                .Where(v => v.CompanyId == companyId && !v.IsDeleted)
                .Select(s => new SelectModel
                {
                    Id = s.Id,
                    Name = s.FullName
                });

            return await vendors.ToListAsync();
        }
    }
}