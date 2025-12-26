namespace EasyAccountingAPI.Repository.Base
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly DatabaseContext _db;
        private IDbContextTransaction? _transaction;

        public UnitOfWorkRepository(DatabaseContext db) => _db = db;

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
                throw new InvalidOperationException("Transaction already started.");

            _transaction = await _db.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction not started.");

            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();

            // IMPORTANT:
            // If DatabaseContext is DI-scoped (recommended), DO NOT dispose it manually.
            // Let DI container dispose it at the end of the request.
            // _db.Dispose();
        }
    }
}