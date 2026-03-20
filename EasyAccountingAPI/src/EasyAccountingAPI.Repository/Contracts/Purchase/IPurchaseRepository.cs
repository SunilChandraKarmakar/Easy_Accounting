namespace EasyAccountingAPI.Repository.Contracts.Purchase
{
    public interface IPurchaseRepository : IBaseRepository<Model.Purchase.Purchase>
    {
        Task<FilterPageResultModel<Model.Purchase.Purchase>> GetPurchasesByFilterAsync(FilterPageModel filterPageModel,
            string? userId, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetPurchaseSelectList(string userId, CancellationToken cancellationToken);
    }
}