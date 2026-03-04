namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.VariationLogic.Queries
{
    public class SelectListVariationQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListVariationQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVariationRepository _variationRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IVariationRepository variationRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _variationRepository = variationRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListVariationQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getVariations = await _variationRepository.GetVariationSelectList(userId, cancellationToken);
                return getVariations;
            }
        }
    }
}