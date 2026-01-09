namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CompanyLogic.Queries
{
    public class SelectListCompanyQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCompanyQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICompanyRepository _companyRepository;

            public Handler(IHttpContextAccessor httpContextAccessor, ICompanyRepository companyRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _companyRepository = companyRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCompanyQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getCompanies = await _companyRepository.GetCompanySelectList(_httpContextAccessor, cancellationToken);
                return getCompanies;
            }
        }
    }
}