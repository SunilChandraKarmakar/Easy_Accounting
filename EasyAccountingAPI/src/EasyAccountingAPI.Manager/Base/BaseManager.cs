namespace EasyAccountingAPI.Manager.Base
{
    public abstract class BaseManager<T> : IBaseManager<T> where T : class
    {
        private readonly IBaseRepository<T> _baseRepository;
        public BaseManager(IBaseRepository<T> baseRepository) => _baseRepository = baseRepository;

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _baseRepository.GetAllAsync();
        }

        public virtual async Task<FilterPageResultModel<T>> GetAllFilterAsync(FilterPageModel model,
            Expression<Func<T, bool>>? filterExpression = null, Expression<Func<T, object>>? defaultSortExpression = null,
            Dictionary<string, Expression<Func<T, object>>>? sortableColumns = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            return await _baseRepository.GetAllFilterAsync(model, filterExpression, defaultSortExpression, sortableColumns, include);
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        public virtual Task CreateAsync(T entity, CancellationToken ct = default)
        {
            return _baseRepository.CreateAsync(entity, ct);
        }

        public virtual Task BulkCreateAsync(IEnumerable<T> entities, CancellationToken ct = default)
        {
            return _baseRepository.BulkCreateAsync(entities, ct);
        }

        public virtual void Update(T entity)
        {
            _baseRepository.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _baseRepository.Delete(entity);
        }
    }
}