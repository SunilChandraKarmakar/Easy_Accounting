namespace EasyAccountingAPI.Repository.Contracts.Purchase
{
    public interface IStorageLocationRepository : IBaseRepository<StorageLocation>
    {
        Task<FilterPageResultModel<StorageLocation>> GetStorageLocationsByFilterAsync(FilterPageModel filterPageModel, 
            string? userId, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetStorageLocationSelectList(string userId, CancellationToken cancellationToken);
    }
}