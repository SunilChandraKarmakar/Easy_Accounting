namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Queries
{
    public class SelectListCurrencyQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCurrencyQuery, IEnumerable<SelectModel>>
        {
            private readonly ICurrencyRepository _currencyRepository;

            public Handler(ICurrencyRepository currencyRepository)
            {
                _currencyRepository = currencyRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCurrencyQuery request, CancellationToken cancellationToken)
            {
                var getCurrencies = await _currencyRepository.GetCurrencySelectList(cancellationToken);
                return getCurrencies;
            }
        }
    }
}