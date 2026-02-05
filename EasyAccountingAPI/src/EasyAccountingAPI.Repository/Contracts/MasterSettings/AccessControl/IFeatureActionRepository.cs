namespace EasyAccountingAPI.Repository.Contracts.MasterSettings.AccessControl
{
    public interface IFeatureActionRepository : IBaseRepository<FeatureAction>
    {
        Task<FilterPageResultModel<FeatureAction>> GetFeatureActionsByFilterAsync(FilterPageModel filterPageModel, 
            CancellationToken cancellationToken);
        Task<List<int>> GetPagedFeatureIdsAsync(FilterPageModel model, CancellationToken cancellationToken);
        Task<int> GetTotalFeatureCountAsync(FilterPageModel model, CancellationToken cancellationToken);
        Task<ICollection<FeatureAction>> GetFeatureActionsByFeatureIdAsync(int featureId, CancellationToken cancellationToken);
    }
}