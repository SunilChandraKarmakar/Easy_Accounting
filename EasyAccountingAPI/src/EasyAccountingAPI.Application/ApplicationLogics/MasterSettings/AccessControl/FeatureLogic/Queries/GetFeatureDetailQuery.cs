namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Queries
{
    public class GetFeatureDetailQuery : IRequest<FeatureUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetFeatureDetailQuery, FeatureUpdateModel>
        {
            private readonly IFeatureRepository _featureRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(IFeatureRepository featureRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _featureRepository = featureRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<FeatureUpdateModel> Handle(GetFeatureDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the feature id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var featureId))
                    return new FeatureUpdateModel();

                // Get feature by id
                var getFeature = await _featureRepository.GetByIdAsync(featureId, cancellationToken);

                if (getFeature is null)
                    return new FeatureUpdateModel();

                // Map feature
                var mapFeature = _mapper.Map<FeatureUpdateModel>(getFeature);
                return mapFeature;
            }
        }
    }
}