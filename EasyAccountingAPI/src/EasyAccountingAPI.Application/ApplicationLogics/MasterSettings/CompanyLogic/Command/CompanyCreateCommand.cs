namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CompanyLogic.Command
{
    public class CompanyCreateCommand : CompanyCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CompanyCreateCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICompanyRepository _companyRepository;
            private readonly IMapper _mapper;            

            public Handler(IHttpContextAccessor httpContextAccessor, IUnitOfWorkRepository unitOfWorkRepository, ICompanyRepository currencyRepository, IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _companyRepository = currencyRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CompanyCreateCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Start Transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Create company
                    var company = _mapper.Map<Company>(request);
                    company.CreatedById = userId;
                    company.CreatedDateTime = DateTime.UtcNow;

                    await _companyRepository.CreateAsync(company, cancellationToken);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return company.Id > 0;
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