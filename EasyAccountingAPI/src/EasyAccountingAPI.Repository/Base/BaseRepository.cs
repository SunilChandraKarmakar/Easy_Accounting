namespace EasyAccountingAPI.Repository.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DatabaseContext db;
        protected BaseRepository(DatabaseContext databaseContext) => db = databaseContext;

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var getAllAsync = await db.Set<T>().ToListAsync();
            return getAllAsync;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var getByIdAsync = await db.Set<T>().FindAsync(id);
            return getByIdAsync;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
            var createAsync = await db.SaveChangesAsync() > 0;

            return entity;
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