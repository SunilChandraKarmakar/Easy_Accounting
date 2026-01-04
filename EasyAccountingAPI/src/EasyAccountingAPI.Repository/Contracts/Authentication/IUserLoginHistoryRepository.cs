namespace EasyAccountingAPI.Repository.Contracts.Authentication
{
    public interface IUserLoginHistoryRepository : IBaseRepository<UserLoginHistory>
    {
        Task<FilterPageResultModel<UserLoginHistory>> GetserLoginHistoriesByFilterAsync(FilterPageModel filterPageModel, 
            CancellationToken cancellationToken);
    }
}