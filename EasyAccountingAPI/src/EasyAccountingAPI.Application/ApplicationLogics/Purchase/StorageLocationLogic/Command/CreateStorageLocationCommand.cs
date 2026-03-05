namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.StorageLocationLogic.Command
{
    public class CreateStorageLocationCommand : StorageLocationCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateStorageLocationCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IStorageLocationRepository _storageLocationRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IStorageLocationRepository storageLocationRepository,
                IUnitOfWorkRepository unitOfWorkRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _storageLocationRepository = storageLocationRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateStorageLocationCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Create storage location 
                var storageLocation = _mapper.Map<StorageLocation>(request);
                storageLocation.CreatedById = userId;
                storageLocation.CreatedDateTime = DateTime.UtcNow;

                await _storageLocationRepository.CreateAsync(storageLocation, cancellationToken);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                return storageLocation.Id > 0;
            }
        }
    }
}