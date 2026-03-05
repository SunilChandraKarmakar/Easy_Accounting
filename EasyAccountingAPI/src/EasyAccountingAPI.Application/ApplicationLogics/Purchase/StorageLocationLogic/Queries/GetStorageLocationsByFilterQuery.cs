namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.StorageLocationLogic.Queries
{
    public class GetStorageLocationsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<StorageLocationGridModel>>
    {
        public class Handler : IRequestHandler<GetStorageLocationsByFilterQuery, FilterPageResultModel<StorageLocationGridModel>>
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

            public async Task<FilterPageResultModel<StorageLocationGridModel>> Handle(GetStorageLocationsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get storage location and map to grid model
                var getStorageLocations = 
                    await _storageLocationRepository.GetStorageLocationsByFilterAsync(request, userId, cancellationToken);
                var mapStorageLocations = _mapper.Map<ICollection<StorageLocationGridModel>>(getStorageLocations.Items);

                // Return paginated result
                return new FilterPageResultModel<StorageLocationGridModel>(mapStorageLocations, getStorageLocations.TotalCount);
            }
        }
    }
}