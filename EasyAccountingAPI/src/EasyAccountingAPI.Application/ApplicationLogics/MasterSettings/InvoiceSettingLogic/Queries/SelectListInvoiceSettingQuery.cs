namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.InvoiceSettingLogic.Queries
{
    public class SelectListInvoiceSettingQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListInvoiceSettingQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IInvoiceSettingRepository _invoiceSettingRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, IInvoiceSettingRepository invoiceSettingRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _invoiceSettingRepository = invoiceSettingRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListInvoiceSettingQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getInvoiceSettings = await _invoiceSettingRepository.GetInvoiceSettingSelectList(_httpContextAccessor, cancellationToken);
                return getInvoiceSettings;
            }
        }
    }
}