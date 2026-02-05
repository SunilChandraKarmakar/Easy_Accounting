namespace EasyAccountingAPI.Repository.Contracts.MasterSettings.AccessControl
{
    public interface IFeatureRepository : IBaseRepository<Feature>
    {
        Task<FilterPageResultModel<Feature>> GetFeaturesByFilterAsync(FilterPageModel filterPageModel, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetFeatureSelectList(CancellationToken cancellationToken);

        // Get features by module id where feature id is not use in the feature action table
        Task<IEnumerable<SelectModel>> GetFeatureSelectListByModuleIdAsync(int moduleId, CancellationToken cancellationToken);
        Task<Feature?> GetFeatureByTableNameAsync(string tableName, CancellationToken cancellationToken);
    }
}