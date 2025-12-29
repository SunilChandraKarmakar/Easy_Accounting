namespace EasyAccountingAPI.Repository.Repository.Global
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        // Get country ids
        public async Task<IEnumerable<int>> GetCountryIdsAsync(CancellationToken cancellationToken)
        {
            var countryIds = await db.Countries
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            return countryIds;
        }

        public override async Task<ICollection<Country>> GetAllAsync(CancellationToken cancellationToken)
        {
            var countries = db.Countries
                .Where(c => !c.IsDeleted)
                .AsQueryable();

            return await countries.ToListAsync(cancellationToken);
        }

        public override async Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var country = await db.Countries
                .Include(c => c.Cities.Where(c => !c.IsDeleted))
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return country;
        }

        // Get countries with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Country>> GetCountriesByFilterAsync(FilterPageModel model, CancellationToken cancellationToken)
        {
            Expression<Func<Country, bool>> filter = c =>
                !c.IsDeleted &&
                (string.IsNullOrWhiteSpace(model.FilterValue)
                 || c.Name.Contains(model.FilterValue)
                 || c.Code.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Country, object>>>
            {
                ["name"] = c => c.Name,
                ["code"] = c => c.Code,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(
                model,
                filter,
                c => c.Id,
                sortableColumns,
                include: q => q.Include(c => c.Cities.Where(c => !c.IsDeleted)), cancellationToken
            );
        }

        public async Task<IEnumerable<SelectModel>> GetCountrySelectList(CancellationToken cancellationToken)
        {
            // Get list of countries for drop down
            var countries = await db.Countries
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync(cancellationToken);

            return countries;
        }
    }
}