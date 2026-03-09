namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Queries
{
    public class GetProductDetailQuery : IRequest<ProductUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetProductDetailQuery, ProductUpdateModel>
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

            public async Task<ProductUpdateModel> Handle(GetProductDetailQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if the product id is null, empty, whitespace, or equals to -1
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id) || request.Id == "-1")
                    return new ProductUpdateModel();

                // Decrypt the product id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var productId))
                    return new ProductUpdateModel();

                // Get product by id
                var getProduct = await _productRepository.GetByIdAsync(productId, cancellationToken);

                if (getProduct is null)
                    return new ProductUpdateModel();

                // Map product
                var mapProduct = _mapper.Map<ProductUpdateModel>(getProduct);
                return mapProduct;
            }
        }
    }
}