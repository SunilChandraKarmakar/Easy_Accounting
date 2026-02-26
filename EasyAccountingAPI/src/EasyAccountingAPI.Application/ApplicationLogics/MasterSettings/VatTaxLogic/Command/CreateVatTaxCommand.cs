namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Command
{
    public class CreateVatTaxCommand : VatTaxCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateVatTaxCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IVatTaxRepository _vatTaxRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IVatTaxRepository vatTaxRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _vatTaxRepository = vatTaxRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateVatTaxCommand request, CancellationToken cancellationToken)
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
                    // Create Vat Tax
                    var vatTax = _mapper.Map<VatTax>(request);
                    vatTax.CreatedById = userId;
                    vatTax.CreatedDateTime = DateTime.UtcNow;
                    await _vatTaxRepository.CreateAsync(vatTax, cancellationToken);

                    // Final save + commit
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return vatTax.Id > 0;
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