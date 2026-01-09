namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Queries
{
    public class SelectListCurrencyQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCurrencyQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICurrencyRepository _currencyRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, ICurrencyRepository currencyRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _currencyRepository = currencyRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCurrencyQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getCurrencies = await _currencyRepository.GetCurrencySelectList(cancellationToken);
                return getCurrencies;
            }
        }
    }
}