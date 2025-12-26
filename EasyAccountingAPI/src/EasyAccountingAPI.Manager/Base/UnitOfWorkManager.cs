namespace EasyAccountingAPI.Manager.Base
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly IUnitOfWorkRepository _uow;
        private bool _disposed;

        public UnitOfWorkManager(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _uow = unitOfWorkRepository;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _uow.SaveChangesAsync(ct);

        public Task BeginTransactionAsync(CancellationToken ct = default)
            => _uow.BeginTransactionAsync(ct);

        public Task CommitTransactionAsync(CancellationToken ct = default)
            => _uow.CommitTransactionAsync(ct);

        public Task RollbackTransactionAsync(CancellationToken ct = default)
            => _uow.RollbackTransactionAsync(ct);

        public void Dispose()
        {
            if (_disposed) return;

            _uow.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}