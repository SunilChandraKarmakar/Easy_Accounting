namespace EasyAccountingAPI.Repository.Repository.Purchase
{
    public class StorageLocationRepository : BaseRepository<StorageLocation>, IStorageLocationRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public StorageLocationRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) 
            : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }

        public override async Task<StorageLocation?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var storageLocation = await db.StorageLocations
                .Where(sl => sl.Id == id && !sl.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return storageLocation;
        }

        // Get storage location with filtering, sorting, and pagination
        public Task<FilterPageResultModel<StorageLocation>> GetStorageLocationsByFilterAsync(FilterPageModel model, 
            string? userId, CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId!, cancellationToken).Result;

            Expression<Func<StorageLocation, bool>> filter = sl =>
                  !sl.IsDeleted
                  && (string.IsNullOrWhiteSpace(userId) || companyIds.Contains(sl.CompanyId))
                  && (string.IsNullOrWhiteSpace(model.FilterValue)
                  || sl.Name.Contains(model.FilterValue)
                  || sl.Company.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<StorageLocation, object>>>
            {
                ["name"] = sl => sl.Name,
                ["company"] = sl => sl.Company.Name,
                ["id"] = sl => sl.Id
            };

            return GetAllFilterAsync(model, filter, sl => sl.Id, sortableColumns,
                include: q => q.Include(x => x.Company), cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetStorageLocationSelectList(string userId, 
            CancellationToken cancellationToken)
        {
            // Get employee based company ids
            var companyIds = await _companyRepository.GetEmployeeBasedCompanyIdsAsync(userId, cancellationToken);

            var getStorageLocations = db.StorageLocations
                .AsNoTracking()
                .Where(sl => companyIds.Contains(sl.CompanyId) && !sl.IsDeleted);

            return await getStorageLocations
                .OrderBy(b => b.Name)
                .Select(s => new SelectModel { Id = s.Id, Name = s.Name })
                .ToListAsync(cancellationToken);
        }
    }
}