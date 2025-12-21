namespace EasyAccountingAPI.Repository.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DatabaseContext db;
        protected BaseRepository(DatabaseContext databaseContext) => db = databaseContext;

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            var getAllAsync = await db.Set<T>().ToListAsync();
            return getAllAsync;
        }

        public virtual async Task<FilterPageResultModel<T>> GetAllFilterAsync(FilterPageModel model, 
            Expression<Func<T, bool>>? filterExpression = null, Expression<Func<T, object>>? defaultSortExpression = null,
            Dictionary<string, Expression<Func<T, object>>>? sortableColumns = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
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

            int totalCount = await query.CountAsync();

            var items = await query
                .Skip(model.PageIndex * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();

            return new FilterPageResultModel<T>(items, totalCount);
        }


        public virtual async Task<T> GetByIdAsync(int id)
        {
            var getByIdAsync = await db.Set<T>().FindAsync(id);
            return getByIdAsync!;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
            var createAsync = await db.SaveChangesAsync() > 0;

            return entity;
        }

        public virtual async Task<int> BulkCreateAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return 0;

            // Improve performance for large inserts
            var previousTracking = db.ChangeTracker.AutoDetectChangesEnabled;
            db.ChangeTracker.AutoDetectChangesEnabled = false;

            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                await db.Set<T>().AddRangeAsync(entities);
                int inserted = await db.SaveChangesAsync();

                await transaction.CommitAsync();
                return inserted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                // Restore original tracking state
                db.ChangeTracker.AutoDetectChangesEnabled = previousTracking;
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            var updateAsync = await db.SaveChangesAsync() > 0;
            db.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            db.Set<T>().Remove(entity);
            var deleteAsync = await db.SaveChangesAsync() > 0;
            db.Entry(entity).State = EntityState.Detached;

            return deleteAsync;
        }
    }
}