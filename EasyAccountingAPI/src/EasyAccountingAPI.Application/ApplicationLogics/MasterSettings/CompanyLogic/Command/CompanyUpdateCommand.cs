namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.CompanyLogic.Command
{
    public class CompanyUpdateCommand : CompanyUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CompanyUpdateCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICompanyRepository _companyRepository;
            private readonly IMapper _mapper;

            public Handler(IHttpContextAccessor httpContextAccessor, IUnitOfWorkRepository unitOfWorkRepository, ICompanyRepository companyRepository, IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _companyRepository = companyRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CompanyUpdateCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing company
                var getExistingCompany = await _companyRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getExistingCompany is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Get login user default company
                    var defaultCompany = await _companyRepository.GetLoginUserDefaultCompany(userId, cancellationToken);

                    // Check, if no default company exist for this user and isDefault company also false then set this as default
                    if (!request.IsDefaultCompany && defaultCompany.Id == request.Id)
                        request.IsDefaultCompany = true;

                    _mapper.Map((CompanyUpdateModel)request, getExistingCompany);
                    getExistingCompany.UpdatedById = userId;
                    getExistingCompany.UpdatedDateTime = DateTime.UtcNow;

                    // Check, if user select default company, remove old default company
                    if (defaultCompany is not null && defaultCompany.Id != getExistingCompany.Id && request.IsDefaultCompany)
                        await _companyRepository.IsRemoveOldDefaultCompanyOfCreatedUser(userId, cancellationToken);

                    _companyRepository.Update(getExistingCompany);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
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