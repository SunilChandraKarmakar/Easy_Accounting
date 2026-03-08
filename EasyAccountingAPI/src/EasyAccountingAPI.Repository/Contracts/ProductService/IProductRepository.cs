namespace EasyAccountingAPI.Repository.Contracts.ProductService
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<FilterPageResultModel<Product>> GetProductsByFilterAsync(FilterPageModel filterPageModel, 
            string? userId, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetProductSelectList(string userId, CancellationToken cancellationToken);
    }
}