namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Queries
{
    public class SelectListVatTaxQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListVatTaxQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVatTaxRepository _vatTaxRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IVatTaxRepository vatTaxRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _vatTaxRepository = vatTaxRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListVatTaxQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getVatTaxes = await _vatTaxRepository.GetVatTaxSelectListAsync(userId, cancellationToken);
                return getVatTaxes;
            }
        }
    }
}