namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Queries
{
    public class GetCurrencyDetailQuery : IRequest<CurrencyUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetCurrencyDetailQuery, CurrencyUpdateModel>
        {
            private readonly ICurrencyRepository _CurrencyRepository;
            private readonly IMapper _mapper;

            public Handler(ICurrencyRepository currencyRepository, IMapper mapper)
            {
                _CurrencyRepository = currencyRepository;
                _mapper = mapper;
            }

            public async Task<CurrencyUpdateModel> Handle(GetCurrencyDetailQuery request, CancellationToken cancellationToken)
            {
                // Decrypt the currency id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var currencyId))
                    return new CurrencyUpdateModel();

                // Get currency by id
                var getCurrency = await _CurrencyRepository.GetByIdAsync(currencyId, cancellationToken);

                if (getCurrency is null)
                    return new CurrencyUpdateModel();

                // Map currency
                var mapCurrency = _mapper.Map<CurrencyUpdateModel>(getCurrency);
                return mapCurrency;
            }
        }
    }
}