namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.StorageLocationLogic.Command
{
    public class DeleteStorageLocationCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteStorageLocationCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IStorageLocationRepository _storageLocationRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IStorageLocationRepository storageLocationRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _storageLocationRepository = storageLocationRepository;
            }

            public async Task<bool> Handle(DeleteStorageLocationCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the storage location id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var storageLocationId))
                    return false;

                // Fetch the storage location
                var storageLocation = await _storageLocationRepository.GetByIdAsync(storageLocationId, cancellationToken);
                if (storageLocation is null)
                    return false;

                storageLocation.IsDeleted = true;
                storageLocation.DeletedDateTime = DateTime.UtcNow;

                _storageLocationRepository.Update(storageLocation);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}