namespace EasyAccountingAPI.Repository.Contracts.MasterSettings
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<FilterPageResultModel<Company>> GetCompaniesByFilterAsync(FilterPageModel filterPageModel, string? userId, 
            CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetCompanySelectList(IHttpContextAccessor httpContextAccessor, 
            CancellationToken cancellationToken);
        Task<bool> IsCreatedUserHaveDefaultCompany(string userId, CancellationToken cancellationToken);
        Task<bool> IsRemoveOldDefaultCompanyOfCreatedUser(string userId, CancellationToken cancellationToken);
        Task<Company> GetLoginUserDefaultCompany(string userId, CancellationToken cancellationToken);
    }
}