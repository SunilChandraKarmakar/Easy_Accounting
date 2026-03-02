namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Queries
{
    public class SelectListCategoryQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListCategoryQuery, IEnumerable<SelectModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICategoryRepository _categoryRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                ICategoryRepository categoryRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _categoryRepository = categoryRepository;
            }

            public async Task<IEnumerable<SelectModel>> Handle(SelectListCategoryQuery request, 
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getCategories = await _categoryRepository.GetCategorySelectList(userId, cancellationToken);
                return getCategories;
            }
        }
    }
}