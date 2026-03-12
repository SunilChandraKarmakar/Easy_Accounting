namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Command
{
    public class UpdateVendorCommand : VendorUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateVendorCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IVendorRepository _vendorRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IVendorRepository vendorRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _vendorRepository = vendorRepository;
                _vendorRepository = vendorRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing vendor
                var getVendor = await _vendorRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getVendor is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    _mapper.Map((VendorUpdateModel)request, getVendor);
                    getVendor.UpdatedById = userId;
                    getVendor.UpdatedDateTime = DateTime.UtcNow;

                    _vendorRepository.Update(getVendor);
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