using System.Drawing.Printing;

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

        // Get countries with filtering, sorting, and pagination
        public async Task<FilterPageResultModel<Country>> GetCountriesByFilterAsync(FilterPageModel filterPageModel)
        {
            // Get countries
            var countries = db.Countries
                .Where(c => !c.IsDeleted)
                .AsNoTracking()
                .AsQueryable();

            // Check if filter value is provided
            if(!string.IsNullOrEmpty(filterPageModel.FilterValue) && !string.IsNullOrWhiteSpace(filterPageModel.FilterValue))
                countries = countries.Where(c => c.Name.ToLower().Contains(filterPageModel.FilterValue.ToLower()) 
                    || c.Code.ToLower().Contains(filterPageModel.FilterValue.ToLower()));

            // Check if sort column and order are provided
            switch(filterPageModel.SortColumn?.ToLower())
            {
                case "name":
                    countries = filterPageModel.SortOrder?.ToLower() == "desc" 
                        ? countries.OrderByDescending(c => c.Name) : countries.OrderBy(c => c.Name);
                    break;
                case "code":
                    countries = filterPageModel.SortOrder?.ToLower() == "desc" 
                        ? countries.OrderByDescending(c => c.Code) : countries.OrderBy(c => c.Code);
                    break;
                default:
                    countries = countries.OrderBy(c => c.Id);
                    break;
            }

            // Apply pagination
            var applyPaginationByCountries = await countries
                .Skip(filterPageModel.PageSize * filterPageModel.PageIndex)
                .Take(filterPageModel.PageSize)
                .ToListAsync();

            // Get total count before pagination
            var totalCount = await countries.CountAsync();

            // Return result
            return new FilterPageResultModel<Country>(applyPaginationByCountries, totalCount);

        }
    }
}