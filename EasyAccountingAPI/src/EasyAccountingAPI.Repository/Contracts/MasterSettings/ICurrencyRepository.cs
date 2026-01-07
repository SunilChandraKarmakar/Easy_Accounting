namespace EasyAccountingAPI.Repository.Contracts.MasterSettings
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
        Task<FilterPageResultModel<Currency>> GetCurrenciesByFilterAsync(FilterPageModel filterPageModel, CancellationToken cancellationToken);
        Task<IEnumerable<SelectModel>> GetCurrencySelectList(CancellationToken cancellationToken);
        Task<bool> AnyAsync(CancellationToken ct);
    }
}