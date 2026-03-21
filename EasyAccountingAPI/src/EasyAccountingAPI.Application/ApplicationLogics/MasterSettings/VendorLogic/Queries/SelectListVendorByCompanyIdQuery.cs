namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Queries
{
    public class SelectListVendorByCompanyIdQuery : IRequest<IEnumerable<SelectModel>>
    {
        public int CompanyId { get; set; }

        public class Handler : IRequestHandler<SelectListVendorByCompanyIdQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVendorRepository _vendorRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IVendorRepository vendorRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _vendorRepository = vendorRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListVendorByCompanyIdQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getVendors = await _vendorRepository.GetVendorsByCompanyIdAsync(request.CompanyId, cancellationToken);
                return getVendors;
            }
        }
    }
}