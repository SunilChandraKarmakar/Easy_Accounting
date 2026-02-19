namespace EasyAccountingAPI.Repository.Contracts.MasterSettings.AccessControl
{
    public interface IActionRepository : IBaseRepository<Model.MasterSettings.AccessControl.Action>
    {
        Task<FilterPageResultModel<Model.MasterSettings.AccessControl.Action>> GetActionsByFilterAsync(
            FilterPageModel filterPageModel, 
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetActionSelectList(CancellationToken cancellationToken);
        Task<bool> IsExistActionByNameAsync(string actionName, CancellationToken cancellationToken);
        Task<bool> AnyAsync(CancellationToken cancellationToken);
    }
}