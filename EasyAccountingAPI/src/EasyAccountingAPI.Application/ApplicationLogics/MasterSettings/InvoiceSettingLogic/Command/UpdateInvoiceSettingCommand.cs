namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.InvoiceSettingLogic.Command
{
    public class UpdateInvoiceSettingCommand : InvoiceSettingUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateInvoiceSettingCommand, bool>
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

            public async Task<bool> Handle(UpdateInvoiceSettingCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing invoice setting
                var getExistingInvoiceSetting = await _invoiceSettingRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getExistingInvoiceSetting is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Get login user default invoice setting
                    var defaultInvoiceSetting = await _invoiceSettingRepository.GetLoginUserDefaultInvoiceSetting(userId, cancellationToken);

                    // Check, if no default invoice setting exist for this user and isDefault invoice setting also false then set this as default
                    if (!request.IsDefaultInvoiceSetting && defaultInvoiceSetting.Id == request.Id)
                        request.IsDefaultInvoiceSetting = true;

                    _mapper.Map((InvoiceSettingUpdateModel)request, getExistingInvoiceSetting);
                    getExistingInvoiceSetting.UpdatedById = userId;
                    getExistingInvoiceSetting.UpdatedDateTime = DateTime.UtcNow;

                    // Check, if user select default invoice setting, remove old default invoice setting
                    if (defaultInvoiceSetting is not null && defaultInvoiceSetting.Id != getExistingInvoiceSetting.Id 
                        && request.IsDefaultInvoiceSetting)
                        await _invoiceSettingRepository.IsRemoveOldDefaultInvoiceSettingOfCreatedUser(userId, cancellationToken);

                    _invoiceSettingRepository.Update(getExistingInvoiceSetting);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
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