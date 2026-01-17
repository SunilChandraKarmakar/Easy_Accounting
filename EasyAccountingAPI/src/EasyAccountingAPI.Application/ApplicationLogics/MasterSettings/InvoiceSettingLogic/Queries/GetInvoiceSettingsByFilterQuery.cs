namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.InvoiceSettingLogic.Queries
{
    public class GetInvoiceSettingsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<InvoiceSettingGridModel>>
    {
        public class Handler : IRequestHandler<GetInvoiceSettingsByFilterQuery, FilterPageResultModel<InvoiceSettingGridModel>>
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

            public async Task<FilterPageResultModel<InvoiceSettingGridModel>> Handle(GetInvoiceSettingsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check, login user is super admin or not
                if (userEmail == "super_admin@gmail.com")
                    userId = null;

                // Get invoice setting and map to grid model
                var getInvoiceSettings = await _invoiceSettingRepository.GetInvoiceSettingsByFilterAsync(request, userId, cancellationToken);

                // Map invoice setting
                var mapInvoiceSettings = _mapper.Map<ICollection<InvoiceSettingGridModel>>(getInvoiceSettings.Items);

                // Return paginated result
                return new FilterPageResultModel<InvoiceSettingGridModel>(mapInvoiceSettings, getInvoiceSettings.TotalCount);
            }
        }
    }
}