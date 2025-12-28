namespace EasyAccountingAPI.Repository.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<FilterPageResultModel<T>> GetAllFilterAsync(FilterPageModel model, Expression<Func<T, bool>>? filterExpression = null,
            Expression<Func<T, object>>? defaultSortExpression = null, Dictionary<string, Expression<Func<T, object>>>? sortableColumns = null, 
            Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task CreateAsync(T entity, CancellationToken cancellationToken);
        Task BulkCreateAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        void Update(T entity);
        void Delete(T entity);
    }
}