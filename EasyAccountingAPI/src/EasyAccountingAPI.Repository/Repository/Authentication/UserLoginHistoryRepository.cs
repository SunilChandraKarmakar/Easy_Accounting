namespace EasyAccountingAPI.Repository.Repository.Authentication
{
    public class UserLoginHistoryRepository : BaseRepository<UserLoginHistory>, IUserLoginHistoryRepository
    {
        public UserLoginHistoryRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        // Get countries with filtering, sorting, and pagination
        public Task<FilterPageResultModel<UserLoginHistory>> GetserLoginHistoriesByFilterAsync(FilterPageModel model, 
            CancellationToken cancellationToken)
        {
            Expression<Func<UserLoginHistory, bool>> filter = x =>
                string.IsNullOrWhiteSpace(model.FilterValue)
                 || x.UserId.Contains(model.FilterValue)
                 || x.LoginIp.Contains(model.FilterValue);

            var sortableColumns = new Dictionary<string, Expression<Func<UserLoginHistory, object>>>
            {
                ["userId"] = x => x.UserId,
                ["loginIp"] = x => x.LoginIp,
                ["id"] = x => x.Id
            };

            return GetAllFilterAsync(model, filter, x => x.Id, sortableColumns, null, cancellationToken);
        }
    }
}