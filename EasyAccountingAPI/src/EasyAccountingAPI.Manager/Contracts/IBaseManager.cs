namespace EasyAccountingAPI.Manager.Contracts
{
    public interface IBaseManager<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<FilterPageResultModel<T>> GetAllFilterAsync(FilterPageModel model, Expression<Func<T, bool>>? filterExpression = null,
            Expression<Func<T, object>>? defaultSortExpression = null, Dictionary<string, Expression<Func<T, object>>>? sortableColumns = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> GetByIdAsync(int id);
        Task CreateAsync(T entity, CancellationToken ct = default);
        Task BulkCreateAsync(IEnumerable<T> entities, CancellationToken ct = default);
        void Update(T entity);
        void Delete(T entity);
    }
}