namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VatTaxLogic.Command
{
    public class UpdateVatTaxCommand : VatTaxUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateVatTaxCommand, bool>
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

            public async Task<bool> Handle(UpdateVatTaxCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing vat tax
                var getExistingVatTax = await _vatTaxRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getExistingVatTax is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    _mapper.Map((VatTaxUpdateModel)request, getExistingVatTax);
                    getExistingVatTax.UpdatedById = userId;
                    getExistingVatTax.UpdatedDateTime = DateTime.UtcNow;
                    _vatTaxRepository.Update(getExistingVatTax);

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