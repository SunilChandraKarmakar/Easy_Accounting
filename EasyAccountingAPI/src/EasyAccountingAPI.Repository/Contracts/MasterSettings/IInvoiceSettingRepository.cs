namespace EasyAccountingAPI.Repository.Contracts.MasterSettings
{
    public interface IInvoiceSettingRepository : IBaseRepository<InvoiceSetting>
    {
        Task<FilterPageResultModel<InvoiceSetting>> GetInvoiceSettingsByFilterAsync(FilterPageModel filterPageModel, string? userId,
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetInvoiceSettingSelectList(IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken);
        Task<bool> IsCreatedUserHaveDefaultInvoiveSetting(string userId, CancellationToken cancellationToken);
        Task<bool> IsRemoveOldDefaultInvoiceSettingOfCreatedUser(string userId, CancellationToken cancellationToken);
        Task<InvoiceSetting> GetLoginUserDefaultInvoiceSetting(string userId, CancellationToken cancellationToken);
    }
}