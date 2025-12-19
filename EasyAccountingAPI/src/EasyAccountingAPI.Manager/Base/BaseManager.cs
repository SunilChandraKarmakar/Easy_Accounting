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

        public virtual async Task<FilterPageResultModel<T>> GetAllFilterAsync(FilterPageModel model, Expression<Func<T, bool>>? filterExpression = null, 
            Expression<Func<T, object>>? defaultSortExpression = null, Dictionary<string, Expression<Func<T, object>>>? sortableColumns = null)
        {
            return await _baseRepository.GetAllFilterAsync(model, filterExpression, defaultSortExpression, sortableColumns);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            return await _baseRepository.CreateAsync(entity);
        }

        public virtual Task<int> BulkCreateAsync(IEnumerable<T> entities)
        {
            return _baseRepository.BulkCreateAsync(entities);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            return await _baseRepository.UpdateAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            return await _baseRepository.DeleteAsync(entity);
        }
    }
}