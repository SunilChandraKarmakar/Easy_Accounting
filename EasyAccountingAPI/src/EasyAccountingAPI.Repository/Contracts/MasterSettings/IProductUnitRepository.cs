namespace EasyAccountingAPI.Repository.Contracts.MasterSettings
{
    public interface IProductUnitRepository : IBaseRepository<ProductUnit>
    {
        Task<FilterPageResultModel<ProductUnit>> GetProductUnitsByFilterAsync(FilterPageModel model,
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetProductUnitSelectListAsync(CancellationToken cancellationToken);
    }
}