namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.BrandLogic.Queries
{
    public class SelectListBrandQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListBrandQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IBrandRepository _brandRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor, 
                IBrandRepository brandRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _brandRepository = brandRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListBrandQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getBrands = await _brandRepository.GetBrandSelectList(userId, cancellationToken);
                return getBrands;
            }
        }
    }
}