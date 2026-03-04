namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.VariationLogic.Queries
{
    public class GetVariationDetailQuery : IRequest<VariationUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetVariationDetailQuery, VariationUpdateModel>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVariationRepository _variationRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IVariationRepository variationRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _variationRepository = variationRepository;
                _mapper = mapper;
            }

            public async Task<VariationUpdateModel> Handle(GetVariationDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if the variation id is null, empty, whitespace, or equals to -1
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id) || request.Id == "-1")
                    return new VariationUpdateModel();

                // Decrypt the variation id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var variationId))
                    return new VariationUpdateModel();

                // Get variation by id
                var getVariation = await _variationRepository.GetByIdAsync(variationId, cancellationToken);

                if (getVariation is null)
                    return new VariationUpdateModel();

                // Map variation
                var mapVariation = _mapper.Map<VariationUpdateModel>(getVariation);
                return mapVariation;
            }
        }
    }
}