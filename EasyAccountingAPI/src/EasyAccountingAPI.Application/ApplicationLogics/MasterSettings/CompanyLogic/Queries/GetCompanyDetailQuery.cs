namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CompanyLogic.Queries
{
    public class GetCompanyDetailQuery : IRequest<CompanyUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetCompanyDetailQuery, CompanyUpdateModel>
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

            public async Task<CompanyUpdateModel> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the company id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var companyId))
                    return new CompanyUpdateModel();

                // Get company by id
                var getCompany = await _companyRepository.GetByIdAsync(companyId, cancellationToken);

                if (getCompany is null)
                    return new CompanyUpdateModel();

                // Map company
                var mapCompany = _mapper.Map<CompanyUpdateModel>(getCompany);
                return mapCompany;
            }
        }
    }
}