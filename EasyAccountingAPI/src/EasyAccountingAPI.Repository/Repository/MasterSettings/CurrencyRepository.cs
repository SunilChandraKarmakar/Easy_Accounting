using Microsoft.EntityFrameworkCore;

namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        // Get currencies with filtering, sorting, and pagination
        public Task<FilterPageResultModel<Currency>> GetCurrenciesByFilterAsync(FilterPageModel model, CancellationToken cancellationToken)
        {
            Expression<Func<Currency, bool>> filter = c =>
                !c.IsDeleted &&
                (string.IsNullOrWhiteSpace(model.FilterValue) || c.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<Currency, object>>>
            {
                ["name"] = c => c.Name,
                ["id"] = c => c.Id
            };

            return GetAllFilterAsync(model, filter, c => c.Id, sortableColumns, include: null, cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetCurrencySelectList(CancellationToken cancellationToken)
        {
            // Get list of currency for dropdown
            var currencies = await db.Currencies
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .Select(c => new SelectModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync(cancellationToken);

            return currencies;
        }

        public async Task<bool> AnyAsync(CancellationToken ct)
        {
            return await db.Currencies
                .AsNoTracking()
                .AnyAsync(c => !c.IsDeleted, ct);
        }
    }
}