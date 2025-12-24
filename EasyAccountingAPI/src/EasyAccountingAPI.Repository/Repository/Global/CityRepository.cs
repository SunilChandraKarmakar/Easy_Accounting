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

        public override async Task<City> GetByIdAsync(int id)
        {
            var cities = await db.Cities
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync();

            return cities!;
        }

        public async Task<bool> DeleteBulkCityByCountryIdAsync(int countryId)
        {
            // Get cities by country id
            var cities = db.Cities
                .AsNoTracking()
                .Where(c => c.CountryId == countryId && !c.IsDeleted);

            // Soft delete cities
            if (cities.Any())
            {
                foreach (var city in cities)
                {
                    city.IsDeleted = true;
                    city.DeletedDateTime = DateTime.UtcNow;
                }

                // Update cities in the database
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}