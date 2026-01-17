namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class InvoiceSettingRepository : BaseRepository<InvoiceSetting>, IInvoiceSettingRepository
    {
        public InvoiceSettingRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<InvoiceSetting?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var invoiceSetting = await db.InvoiceSettings
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return invoiceSetting;
        }

        // Get invoice setting with filtering, sorting, and pagination
        public Task<FilterPageResultModel<InvoiceSetting>> GetInvoiceSettingsByFilterAsync(FilterPageModel model, string? userId,
            CancellationToken cancellationToken)
        {
            Expression<Func<InvoiceSetting, bool>> filter = ic =>
                 !ic.IsDeleted
                 && (string.IsNullOrWhiteSpace(userId) || ic.CreatedById == userId)
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || ic.InvoiceFooter.Contains(model.FilterValue)
                 || ic.InvoiceTerms.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<InvoiceSetting, object>>>
            {
                ["invoiceFooter"] = ic => ic.InvoiceFooter,
                ["invoiceTerms"] = ic => ic.InvoiceTerms,
                ["id"] = ic => ic.Id
            };

            return GetAllFilterAsync(model, filter, ic => ic.Id, sortableColumns, include: null, cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetInvoiceSettingSelectList(IHttpContextAccessor httpContextAccessor, 
            CancellationToken cancellationToken)
        {
            // Retrieve the login user's info from the current HTTP context
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;

            var getInvoiceSettings = db.InvoiceSettings.AsNoTracking().Where(c => !c.IsDeleted);

            if (userEmail is not "super_admin@gmail.com")
                getInvoiceSettings = getInvoiceSettings.Where(c => c.CreatedById == userId);

            return await getInvoiceSettings
                .Select(c => new SelectModel { Id = c.Id, Name = c.InvoiceDueDateCount.ToString() })
                .ToListAsync(cancellationToken);
        }

        // Check, invoice setting created user has default invoice setting or not
        public async Task<bool> IsCreatedUserHaveDefaultInvoiveSetting(string userId, CancellationToken cancellationToken)
        {
            return await db.InvoiceSettings
                .AsNoTracking()
                .AnyAsync(x => !x.IsDeleted && x.IsDefaultInvoiceSetting && x.CreatedById == userId, cancellationToken);
        }

        // Check, if create user select default invoice setting then remove old default invoice setting
        public async Task<bool> IsRemoveOldDefaultInvoiceSettingOfCreatedUser(string userId, CancellationToken cancellationToken)
        {
            var oldDefaultInvoiceSetting = await db.InvoiceSettings
                .Where(c => c.CreatedById == userId && c.IsDefaultInvoiceSetting && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (oldDefaultInvoiceSetting is not null)
            {
                oldDefaultInvoiceSetting.IsDefaultInvoiceSetting = false;
                db.InvoiceSettings.Update(oldDefaultInvoiceSetting);
                return true;
            }

            return false;
        }

        // Get login user's default invoice setting
        public async Task<InvoiceSetting> GetLoginUserDefaultInvoiceSetting(string userId, CancellationToken cancellationToken)
        {
            var defaultInvoiceSetting = await db.InvoiceSettings
                .Where(c => c.CreatedById == userId && c.IsDefaultInvoiceSetting && !c.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return defaultInvoiceSetting!;
        }
    }
}