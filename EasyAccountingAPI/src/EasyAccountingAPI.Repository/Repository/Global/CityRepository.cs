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

        public async Task<int> DeleteBulkCityByCountryIdAsync(int countryId)
        {
            return await db.Cities
                .Where(c => c.CountryId == countryId && !c.IsDeleted)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsDeleted, true)
                .SetProperty(c => c.DeletedDateTime, DateTime.UtcNow));
        }
    }
}