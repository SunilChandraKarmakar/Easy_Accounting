namespace EasyAccountingAPI.Manager.Base
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private bool _disposed;

        public UnitOfWorkManager(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _unitOfWorkRepository.GetRepository<TRepository>();
        }

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return _unitOfWorkRepository.CommitAsync(cancellationToken);
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return _unitOfWorkRepository.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _unitOfWorkRepository.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}