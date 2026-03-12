namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Queries
{
    public class SelectListVendorQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListVendorQuery, IEnumerable<SelectModel>>
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

            public async Task<IEnumerable<SelectModel>> Handle(SelectListVendorQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getVendors = await _vendorRepository.GetVendorSelectList(userId, cancellationToken);
                return getVendors;
            }
        }
    }
}