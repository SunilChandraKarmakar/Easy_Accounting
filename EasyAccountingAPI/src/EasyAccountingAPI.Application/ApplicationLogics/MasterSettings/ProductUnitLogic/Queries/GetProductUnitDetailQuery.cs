namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Queries
{
    public class GetProductUnitDetailQuery : IRequest<ProductUnitUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetProductUnitDetailQuery, ProductUnitUpdateModel>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IProductUnitRepository _productUnitRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IProductUnitRepository productUnitRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _productUnitRepository = productUnitRepository;
                _mapper = mapper;
            }

            public async Task<ProductUnitUpdateModel> Handle(GetProductUnitDetailQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the product unit id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var productUnitId))
                    return new ProductUnitUpdateModel();

                // Get product unit by id
                var getProductUnit = await _productUnitRepository.GetByIdAsync(productUnitId, cancellationToken);

                if (getProductUnit is null)
                    return new ProductUnitUpdateModel();

                // Map product unit
                var mapProductUnit = _mapper.Map<ProductUnitUpdateModel>(getProductUnit);
                return mapProductUnit;
            }
        }
    }
}