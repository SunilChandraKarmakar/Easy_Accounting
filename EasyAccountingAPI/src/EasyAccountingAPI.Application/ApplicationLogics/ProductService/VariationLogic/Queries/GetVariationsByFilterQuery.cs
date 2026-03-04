namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.VariationLogic.Queries
{
    public class GetVariationsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<VariationGridModel>>
    {
        public class Handler : IRequestHandler<GetVariationsByFilterQuery, FilterPageResultModel<VariationGridModel>>
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

            public async Task<FilterPageResultModel<VariationGridModel>> Handle(GetVariationsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get variation and map to grid model
                var getVariations = await _variationRepository.GetVariationsByFilterAsync(request, userId, cancellationToken);
                var mapVariations = _mapper.Map<ICollection<VariationGridModel>>(getVariations.Items);

                // Return paginated result
                return new FilterPageResultModel<VariationGridModel>(mapVariations, getVariations.TotalCount);
            }
        }
    }
}