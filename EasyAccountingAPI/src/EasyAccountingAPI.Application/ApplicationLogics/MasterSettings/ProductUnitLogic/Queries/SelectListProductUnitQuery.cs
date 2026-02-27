namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Queries
{
    public class SelectListProductUnitQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListProductUnitQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IProductUnitRepository _productUnitRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IProductUnitRepository productUnitRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _productUnitRepository = productUnitRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListProductUnitQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getProductUnits = await _productUnitRepository.GetProductUnitSelectListAsync(cancellationToken);
                return getProductUnits;
            }
        }
    }
}