namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Queries
{
    public class SelectListProductQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListProductQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IProductRepository _productRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IProductRepository productRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _productRepository = productRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListProductQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getProducts = await _productRepository.GetProductSelectList(userId, cancellationToken);
                return getProducts;
            }
        }
    }
}