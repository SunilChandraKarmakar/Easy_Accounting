namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CurrencyLogic.Command
{
    public class DeleteCurrencyCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteCurrencyCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICurrencyRepository _currencyRepository;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, ICurrencyRepository currencyRepository)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _currencyRepository = currencyRepository;
            }

            public async Task<bool> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
            {
                // Decrypt the currency id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var currencyId))
                    return false;

                // Fetch the currency
                var currency = await _currencyRepository.GetByIdAsync(currencyId, cancellationToken);
                if (currency is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    currency.IsDeleted = true;
                    currency.DeletedDateTime = DateTime.UtcNow;
                    _currencyRepository.Update(currency);

                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}