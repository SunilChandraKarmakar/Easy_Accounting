namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.VendorLogic.Command
{
    public class DeleteVendorCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteVendorCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IVendorRepository _vendorRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IVendorRepository vendorRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _vendorRepository = vendorRepository;
            }

            public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the vendor id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var vendorId))
                    return false;

                // Fetch the vendor
                var vendor = await _vendorRepository.GetByIdAsync(vendorId, cancellationToken);
                if (vendor is null)
                    return false;

                vendor.IsDeleted = true;
                vendor.DeletedDateTime = DateTime.UtcNow;

                // Remove the vendor address
                foreach (var vendorAddress in vendor.VendorAddresses)
                    vendor.VendorAddresses.Remove(vendorAddress);

                _vendorRepository.Update(vendor);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}