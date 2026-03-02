namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.BrandLogic.Queries
{
    public class GetBrandDetailQuery : IRequest<BrandUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetBrandDetailQuery, BrandUpdateModel>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IBrandRepository brandRepository,                 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            public async Task<BrandUpdateModel> Handle(GetBrandDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if the brand id is null, empty, whitespace, or equals to -1
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id) || request.Id == "-1")
                    return new BrandUpdateModel();

                // Decrypt the brand id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var brandId))
                    return new BrandUpdateModel();

                // Get brand by id
                var getBrand = await _brandRepository.GetByIdAsync(brandId, cancellationToken);

                if (getBrand is null)
                    return new BrandUpdateModel();

                // Map brand
                var mapBrand = _mapper.Map<BrandUpdateModel>(getBrand);
                return mapBrand;
            }
        }
    }
}