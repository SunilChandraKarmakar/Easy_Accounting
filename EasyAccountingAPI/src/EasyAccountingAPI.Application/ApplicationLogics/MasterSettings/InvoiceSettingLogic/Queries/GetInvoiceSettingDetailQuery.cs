namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.InvoiceSettingLogic.Queries
{
    public class GetInvoiceSettingDetailQuery : IRequest<InvoiceSettingUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetInvoiceSettingDetailQuery, InvoiceSettingUpdateModel>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IInvoiceSettingRepository _invoiceSettingRepository;
            private readonly IMapper _mapper;

            public Handler(IHttpContextAccessor httpContextAccessor, IInvoiceSettingRepository invoiceSettingRepository, IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _invoiceSettingRepository = invoiceSettingRepository;
                _mapper = mapper;
            }

            public async Task<InvoiceSettingUpdateModel> Handle(GetInvoiceSettingDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the invoice setting id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var invoiceSettingId))
                    return new InvoiceSettingUpdateModel();

                // Get invoice setting by id
                var getInvoiceSetting = await _invoiceSettingRepository.GetByIdAsync(invoiceSettingId, cancellationToken);

                if (getInvoiceSetting is null)
                    return new InvoiceSettingUpdateModel();

                // Map invoice setting
                var mapInvoiceSetting = _mapper.Map<InvoiceSettingUpdateModel>(getInvoiceSetting);
                return mapInvoiceSetting;
            }
        }
    }
}