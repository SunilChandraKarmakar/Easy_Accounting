namespace EasyAccountingAPI.Repository.Repository.Global
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<City>> GetAllAsync()
        {
            var cities = db.Cities
                .Where(c => !c.IsDeleted)
                .AsQueryable();

            return await cities.ToListAsync();
        }

        // Get cities with filtering, sorting, and pagination
        public Task<FilterPageResultModel<City>> GetCitiesByFilterAsync(FilterPageModel model)
        {
            Expression<Func<City, bool>> filter = c =>
                !c.IsDeleted &&
                (string.IsNullOrWhiteSpace(model.FilterValue)
                 || c.Name.Contains(model.FilterValue)
                 || c.Country.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<City, object>>>
            {
                ["name"] = c => c.Name,
                ["country"] = c => c.Country.Name,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(
                model,
                filter,
                c => c.Id,
                sortableColumns,
                include: q => q.Include(c => c.Country)
            );
        }

        public override async Task<City?> GetByIdAsync(int id)
        {
            var cities = await db.Cities
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync();

            return cities;
        }

        public async Task<int> DeleteBulkCityByCountryIdAsync(int countryId)
        {
            return await db.Cities
                .Where(c => c.CountryId == countryId && !c.IsDeleted)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsDeleted, true)
                .SetProperty(c => c.DeletedDateTime, DateTime.UtcNow));
        }
    }
}