namespace EasyAccountingAPI.Repository.Base
{
    public abstract class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly DatabaseContext _db;
        private readonly IServiceProvider _serviceProvider;
        private IDbContextTransaction? _transaction;

        public UnitOfWorkRepository(DatabaseContext db, IServiceProvider serviceProvider)
        {
            _db = db;
            _serviceProvider = serviceProvider;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            var repository = _serviceProvider.GetService<TRepository>();

            if (repository is null)
                throw new InvalidOperationException($"Repository {typeof(TRepository).Name} not registered.");

            return repository;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction not started.");

            await _db.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                await _transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _db.Dispose();
        }
    }
}