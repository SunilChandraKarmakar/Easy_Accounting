namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Command
{
    public class UpdateCurrencyCommand : CurrencyUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateCurrencyCommand, bool>
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

            public async Task<bool> Handle(UpdateCurrencyCommand request, CancellationToken ct)
            {
                // Fetch existing currency
                var getExistingCurrency = await _currencyRepository.GetByIdAsync(request.Id, ct);
                if (getExistingCurrency is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(ct);

                try
                {
                    _mapper.Map((CurrencyUpdateModel)request, getExistingCurrency);
                    _currencyRepository.Update(getExistingCurrency);
                    await _unitOfWorkRepository.SaveChangesAsync(ct);
                    await _unitOfWorkRepository.CommitTransactionAsync(ct);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(ct);
                    return false;
                }
            }
        }
    }
}