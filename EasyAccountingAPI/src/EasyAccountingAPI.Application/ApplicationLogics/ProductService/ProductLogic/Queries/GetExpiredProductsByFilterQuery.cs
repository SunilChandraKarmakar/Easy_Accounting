namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Queries
{
    public class GetExpiredProductsByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<ProductGridModel>>
    {
        public class Handler : IRequestHandler<GetExpiredProductsByFilterQuery, FilterPageResultModel<ProductGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IProductRepository productRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<ProductGridModel>> Handle(GetExpiredProductsByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get product and map to grid model
                var getProducts = await _productRepository.GetExpiredProductsByFilterAsync(request, userId, cancellationToken);
                var mapProducts = _mapper.Map<ICollection<ProductGridModel>>(getProducts.Items);

                // Return paginated result
                return new FilterPageResultModel<ProductGridModel>(mapProducts, getProducts.TotalCount);
            }
        }
    }
}