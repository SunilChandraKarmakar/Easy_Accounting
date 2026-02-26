namespace EasyAccountingAPI.Repository.Contracts.MasterSettings
{
    public interface IVatTaxRepository : IBaseRepository<VatTax>
    {
        Task<FilterPageResultModel<VatTax>> GetVatTaxesByFilterAsync(FilterPageModel model, 
            string? userId, CancellationToken cancellationToken);
    }
}