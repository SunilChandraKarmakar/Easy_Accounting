namespace EasyAccountingAPI.Repository.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DatabaseContext db;
        protected BaseRepository(DatabaseContext databaseContext) => db = databaseContext;

        public virtual async Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            var getAllAsync = await db.Set<T>().ToListAsync(cancellationToken);
            return getAllAsync;
        }

        public virtual async Task<FilterPageResultModel<T>> GetAllFilterAsync(FilterPageModel model, 
            Expression<Func<T, bool>>? filterExpression = null, Expression<Func<T, object>>? defaultSortExpression = null,
            Dictionary<string, Expression<Func<T, object>>>? sortableColumns = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = db.Set<T>().AsNoTracking();

            // Apply Include
            if (include is not null)
                query = include(query);

            // Apply filtering
            if (filterExpression != null)
                query = query.Where(filterExpression);

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(model.SortColumn) && sortableColumns is not null 
                && sortableColumns.TryGetValue(model.SortColumn.ToLower(), out var sortExpression))
                query = model.SortOrder?.ToLower() == "descend" ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);
            else if (defaultSortExpression is not null)
                query = query.OrderBy(defaultSortExpression);

            int totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip(model.PageIndex * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync(cancellationToken);

            return new FilterPageResultModel<T>(items, totalCount);
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var getByIdAsync = await db.Set<T>().FindAsync(id, cancellationToken);
            return getByIdAsync;
        }

        public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await db.Set<T>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task BulkCreateAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            if (entities == null) return;
            await db.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual void Update(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            db.Set<T>().Remove(entity);
        }

        public virtual async Task BulkDeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            if (entities == null) return;
            db.Set<T>().RemoveRange(entities);
            await Task.CompletedTask;
        }
    }
}