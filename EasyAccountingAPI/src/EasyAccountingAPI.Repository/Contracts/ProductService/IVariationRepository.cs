namespace EasyAccountingAPI.Repository.Contracts.ProductService
{
    public interface IVariationRepository : IBaseRepository<Variation>
    {
        Task<FilterPageResultModel<Variation>> GetVariationsByFilterAsync(FilterPageModel filterPageModel, string? userId,
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetVariationSelectList(string userId, CancellationToken cancellationToken);
    }
}