namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Command
{
    public class CreateCurrencyCommand : CurrencyCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCurrencyCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICurrencyRepository _currencyRepository;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICurrencyRepository currencyRepository, IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _currencyRepository = currencyRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
            {
                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create currency
                    var currency = _mapper.Map<Currency>(request);
                    await _currencyRepository.CreateAsync(currency, cancellationToken);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return currency.Id > 0;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}