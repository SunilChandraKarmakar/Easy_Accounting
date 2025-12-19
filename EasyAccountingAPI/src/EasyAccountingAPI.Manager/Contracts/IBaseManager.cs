namespace EasyAccountingAPI.Manager.Contracts
{
    public interface IBaseManager<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<FilterPageResultModel<T>> GetAllFilterAsync(FilterPageModel model, Expression<Func<T, bool>>? filterExpression = null, Expression<Func<T, object>>? defaultSortExpression = null,
           Dictionary<string, Expression<Func<T, object>>>? sortableColumns = null);
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<int> BulkCreateAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}