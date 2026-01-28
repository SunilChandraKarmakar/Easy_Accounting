namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Queries
{
    public class GetCurrenciesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<CurrencyGridModel>>
    {
        public class Handler : IRequestHandler<GetCurrenciesByFilterQuery, FilterPageResultModel<CurrencyGridModel>>
        {
            private readonly ICurrencyRepository _currencyRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(ICurrencyRepository currencyRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _currencyRepository = currencyRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<CurrencyGridModel>> Handle(GetCurrenciesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get currency and map to grid model
                var getCurrencies = await _currencyRepository.GetCurrenciesByFilterAsync(request, cancellationToken);
                var mapCurrency = _mapper.Map<ICollection<CurrencyGridModel>>(getCurrencies.Items);

                // Return paginated result
                return new FilterPageResultModel<CurrencyGridModel>(mapCurrency, getCurrencies.TotalCount);
            }
        }
    }
}