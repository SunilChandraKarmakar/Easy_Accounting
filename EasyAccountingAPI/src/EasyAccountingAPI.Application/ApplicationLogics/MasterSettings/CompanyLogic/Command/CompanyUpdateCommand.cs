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

            public async Task<bool> Handle(CompanyUpdateCommand request, CancellationToken ct)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing company
                var getExistingCompany = await _companyRepository.GetByIdAsync(request.Id, ct);
                if (getExistingCompany is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(ct);

                try
                {
                    _mapper.Map((CompanyUpdateModel)request, getExistingCompany);
                    getExistingCompany.UpdatedById = userId;
                    getExistingCompany.UpdatedDateTime = DateTime.UtcNow;

                    _companyRepository.Update(getExistingCompany);
                    await _unitOfWorkRepository.SaveChangesAsync(ct);
                    await _unitOfWorkRepository.CommitTransactionAsync(ct);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(ct);
                    return false;
                }
            }
        }
    }
}