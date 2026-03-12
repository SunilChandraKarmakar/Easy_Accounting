namespace EasyAccountingAPI.Repository.Contracts.MasterSettings.VendorInformation
{
    public interface IVendorRepository : IBaseRepository<Vendor>
    {
        Task<FilterPageResultModel<Vendor>> GetVendorsByFilterAsync(FilterPageModel filterPageModel, string? userId,
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetVendorSelectList(string userId, CancellationToken cancellationToken);
    }
}