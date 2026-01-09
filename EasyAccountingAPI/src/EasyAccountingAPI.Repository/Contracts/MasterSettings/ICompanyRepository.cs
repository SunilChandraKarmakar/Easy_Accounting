namespace EasyAccountingAPI.Repository.Contracts.MasterSettings
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<FilterPageResultModel<Company>> GetCompaniesByFilterAsync(FilterPageModel filterPageModel, string userId, bool isSuperAdmin, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetCompanySelectList(IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken);
    }
}