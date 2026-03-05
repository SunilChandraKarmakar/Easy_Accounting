namespace EasyAccountingAPI.Application.ApplicationLogics.Purchase.StorageLocationLogic.Queries
{
    public class SelectListStorageLocationQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListStorageLocationQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IStorageLocationRepository _storageLocationRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IStorageLocationRepository storageLocationRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _storageLocationRepository = storageLocationRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListStorageLocationQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getStorageLocations = await _storageLocationRepository.GetStorageLocationSelectList(userId, cancellationToken);
                return getStorageLocations;
            }
        }
    }
}