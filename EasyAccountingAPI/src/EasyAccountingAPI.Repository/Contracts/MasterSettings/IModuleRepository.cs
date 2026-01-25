namespace EasyAccountingAPI.Repository.Contracts.MasterSettings
{
    public interface IModuleRepository : IBaseRepository<Module>
    {
        Task<FilterPageResultModel<Module>> GetModulesByFilterAsync(FilterPageModel filterPageModel, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetModuleSelectList(CancellationToken cancellationToken);
        Task<bool> AnyAsync(CancellationToken cancellationToken);
    }
}