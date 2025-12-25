namespace EasyAccountingAPI.Repository.Contracts
{
    public interface IUnitOfWorkRepository : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class;
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}