namespace EasyAccountingAPI.Manager.Contracts
{
    public interface IUnitOfWorkManager : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class;
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}