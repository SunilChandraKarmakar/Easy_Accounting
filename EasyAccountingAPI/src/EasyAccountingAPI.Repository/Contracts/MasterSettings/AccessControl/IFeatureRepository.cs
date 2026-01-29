namespace EasyAccountingAPI.Repository.Contracts.MasterSettings.AccessControl
{
    public interface IFeatureRepository : IBaseRepository<Feature>
    {
        Task<FilterPageResultModel<Feature>> GetFeaturesByFilterAsync(FilterPageModel filterPageModel, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetFeatureSelectListByModule(int moduleId, CancellationToken cancellationToken);
        Task<Feature?> GetFeatureByTableNameAsync(string tableName, CancellationToken cancellationToken);
    }
}