namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.StorageLocationLogic.Queries
{
    public class GetStorageLocationDetailQuery : IRequest<StorageLocationUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetStorageLocationDetailQuery, StorageLocationUpdateModel>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IStorageLocationRepository _storageLocationRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IStorageLocationRepository storageLocationRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _storageLocationRepository = storageLocationRepository;
                _mapper = mapper;
            }

            public async Task<StorageLocationUpdateModel> Handle(GetStorageLocationDetailQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if the storage location id is null, empty, whitespace, or equals to -1
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id) || request.Id == "-1")
                    return new StorageLocationUpdateModel();

                // Decrypt the storage location id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var storageLocationId))
                    return new StorageLocationUpdateModel();

                // Get storage location by id
                var getStorageLocation = await _storageLocationRepository.GetByIdAsync(storageLocationId, cancellationToken);

                if (getStorageLocation is null)
                    return new StorageLocationUpdateModel();

                // Map storage location
                var mapStorageLocation = _mapper.Map<StorageLocationUpdateModel>(getStorageLocation);
                return mapStorageLocation;
            }
        }
    }
}