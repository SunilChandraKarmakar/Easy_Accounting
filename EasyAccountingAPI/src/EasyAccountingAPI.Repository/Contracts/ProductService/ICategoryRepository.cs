namespace EasyAccountingAPI.Repository.Contracts.ProductService
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<FilterPageResultModel<Category>> GetCategoriesByFilterAsync(FilterPageModel filterPageModel, string? userId,
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetCategorySelectList(string userId, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetParentCategorySelectList(string userId, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetCategorySelectListByParentIdAsync(int parentId, string userId, 
            CancellationToken cancellationToken);
    }
}