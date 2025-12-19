namespace EasyAccountingAPI.Repository.Repository.Global
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        // Get country ids
        public async Task<IEnumerable<int>> GetCountryIdsAsync()
        {
            var countryIds = await db.Countries
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Select(c => c.Id)
                .ToListAsync();

            return countryIds;
        }

        public override async Task<ICollection<Country>> GetAllAsync()
        {
            var countries = db.Countries
                .Where(c => !c.IsDeleted)
                .AsQueryable();

            return await countries.ToListAsync();
        }

        public override async Task<Country> GetByIdAsync(int id)
        {
            var country = await db.Countries
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync();

            return country!;
        }

        // Get countries with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Country>> GetCountriesByFilterAsync(FilterPageModel model)
        {
            // Define filter expression
            Expression<Func<Country, bool>> filter = c =>
                !c.IsDeleted 
                && (string.IsNullOrEmpty(model.FilterValue)
                && (string.IsNullOrWhiteSpace(model.FilterValue))
                || c.Name.Contains(model.FilterValue) 
                || c.Code.Contains(model.FilterValue));

            // Define sortable columns
            var sortableColumns = new Dictionary<string, Expression<Func<Country, object>>>
            {
                ["name"] = c => c.Name,
                ["code"] = c => c.Code,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, c => c.Id, sortableColumns);
        }
    }
}