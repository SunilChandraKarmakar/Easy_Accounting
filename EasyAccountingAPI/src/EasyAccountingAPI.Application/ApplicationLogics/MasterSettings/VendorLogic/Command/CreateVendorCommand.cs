namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Command
{
    public class CreateVendorCommand : VendorCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateVendorCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVendorRepository _vendorRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IVendorRepository vendorRepository,
                IUnitOfWorkRepository unitOfWorkRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _vendorRepository = vendorRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
            {
                // Get the user ID from the HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Start a transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    var vendor = _mapper.Map<Vendor>(request);
                    vendor.CreatedById = userId;
                    vendor.CreatedDateTime = DateTime.UtcNow;

                    await _vendorRepository.CreateAsync(vendor, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return vendor.Id > 0;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}