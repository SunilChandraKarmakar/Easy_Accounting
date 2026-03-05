namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.StorageLocationLogic.Command
{
    public class UpdateStorageLocationCommand : StorageLocationUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateStorageLocationCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IStorageLocationRepository _storageLocationRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IStorageLocationRepository storageLocationRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _storageLocationRepository = storageLocationRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateStorageLocationCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing storage location
                var getStorageLocation = await _storageLocationRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getStorageLocation is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    _mapper.Map((StorageLocationUpdateModel)request, getStorageLocation);
                    getStorageLocation.UpdatedById = userId;
                    getStorageLocation.UpdatedDateTime = DateTime.UtcNow;

                    _storageLocationRepository.Update(getStorageLocation);
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