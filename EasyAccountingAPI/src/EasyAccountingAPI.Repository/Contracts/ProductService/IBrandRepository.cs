namespace EasyAccountingAPI.Repository.Contracts.ProductService
{
    public interface IBrandRepository : IBaseRepository<Brand>
    {
        Task<FilterPageResultModel<Brand>> GetBrandsByFilterAsync(FilterPageModel filterPageModel, string? userId,
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetBrandSelectList(string userId, CancellationToken cancellationToken);
    }
}