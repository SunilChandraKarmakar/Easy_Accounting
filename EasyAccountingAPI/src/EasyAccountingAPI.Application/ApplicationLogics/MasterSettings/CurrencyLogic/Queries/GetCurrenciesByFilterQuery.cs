namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Queries
{
    public class GetCurrenciesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<CurrencyGridModel>>
    {
        public class Handler : IRequestHandler<GetCurrenciesByFilterQuery, FilterPageResultModel<CurrencyGridModel>>
        {
            private readonly ICurrencyRepository _currencyRepository;
            private readonly IMapper _mapper;

            public Handler(ICurrencyRepository currencyRepository, IMapper mapper)
            {
                _currencyRepository = currencyRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<CurrencyGridModel>> Handle(GetCurrenciesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Get currency and map to grid model
                var getCurrencies = await _currencyRepository.GetCurrenciesByFilterAsync(request, cancellationToken);
                var mapCurrency = _mapper.Map<ICollection<CurrencyGridModel>>(getCurrencies.Items);

                // Return paginated result
                return new FilterPageResultModel<CurrencyGridModel>(mapCurrency, getCurrencies.TotalCount);
            }
        }
    }
}