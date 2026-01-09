namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CompanyLogic.Queries
{
    public class GetCompaniesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<CompanyGridModel>>
    {
        public class Handler : IRequestHandler<GetCompaniesByFilterQuery, FilterPageResultModel<CompanyGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICompanyRepository _companyRepository;
            private readonly IMapper _mapper;

            public Handler(IHttpContextAccessor httpContextAccessor, ICompanyRepository companyRepository, IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _companyRepository = companyRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<CompanyGridModel>> Handle(GetCompaniesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get currency and map to grid model
                var getCompany = await _companyRepository.GetCompaniesByFilterAsync(request, cancellationToken);

                // Map company
                var mapCompany = _mapper.Map<ICollection<CompanyGridModel>>(getCompany.Items);

                // Return paginated result
                return new FilterPageResultModel<CompanyGridModel>(mapCompany, getCompany.TotalCount);
            }
        }
    }
}