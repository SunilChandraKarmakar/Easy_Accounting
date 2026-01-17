namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.InvoiceSettingLogic.Command
{
    public class DeleteInvoiceSettingCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteInvoiceSettingCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IInvoiceSettingRepository _invoiceSettingRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IUnitOfWorkRepository unitOfWorkRepository, 
                IInvoiceSettingRepository invoiceSettingRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _invoiceSettingRepository = invoiceSettingRepository;
            }

            public async Task<bool> Handle(DeleteInvoiceSettingCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the invoice setting id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var invoiceSettingId))
                    return false;

                // Fetch the invoice setting
                var invoiceSetting = await _invoiceSettingRepository.GetByIdAsync(invoiceSettingId, cancellationToken);
                if (invoiceSetting is null)
                    return false;

                // Begin transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    invoiceSetting.IsDeleted = true;
                    invoiceSetting.DeletedDateTime = DateTime.UtcNow;
                    _invoiceSettingRepository.Update(invoiceSetting);

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