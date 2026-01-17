namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.InvoiceSettingLogic.Command
{
    public class CreateInvoiceSettingCommand : InvoiceSettingCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateInvoiceSettingCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IInvoiceSettingRepository _invoiceSettingRepository;
            private readonly IMapper _mapper;

            public Handler(IHttpContextAccessor httpContextAccessor, IUnitOfWorkRepository unitOfWorkRepository, 
                IInvoiceSettingRepository invoiceSettingRepository, IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _invoiceSettingRepository = invoiceSettingRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateInvoiceSettingCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create Invoice Setting
                    var invoiceSetting = _mapper.Map<InvoiceSetting>(request);
                    invoiceSetting.CreatedById = userId;
                    invoiceSetting.CreatedDateTime = DateTime.UtcNow;

                    // If user select this as default invoice setting, remove old default invoice setting
                    if (invoiceSetting.IsDefaultInvoiceSetting)
                        await _invoiceSettingRepository.IsRemoveOldDefaultInvoiceSettingOfCreatedUser(userId, cancellationToken);

                    // Check, if created user has no invoice setting yet, set this as default
                    if (!await _invoiceSettingRepository.IsCreatedUserHaveDefaultInvoiveSetting(userId, cancellationToken))
                        invoiceSetting.IsDefaultInvoiceSetting = true;

                    await _invoiceSettingRepository.CreateAsync(invoiceSetting, cancellationToken);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return invoiceSetting.Id > 0;
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