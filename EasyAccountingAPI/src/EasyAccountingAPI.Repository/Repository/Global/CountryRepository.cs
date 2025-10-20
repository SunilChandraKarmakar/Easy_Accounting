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
    }
}