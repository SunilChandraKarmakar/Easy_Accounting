namespace EasyAccountingAPI.Repository.Repository.Global
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(DatabaseContext databaseContext) : base(databaseContext) { }

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

        public async Task<FilterPagedResult<Country>> GetCountriesFilterAsync(int pageNumber, int pageSize)
        {
            // Get countries
            var countries = db.Countries.Where(c => !c.IsDeleted).OrderBy(c => c.Id);

            // Get total count of countries
            var totalCountCountries = await countries.CountAsync();

            // Get filtered countries
            var filterCountries = await countries
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new FilterPagedResult<Country>(filterCountries, totalCountCountries, pageNumber, pageSize);
        }
    }
}