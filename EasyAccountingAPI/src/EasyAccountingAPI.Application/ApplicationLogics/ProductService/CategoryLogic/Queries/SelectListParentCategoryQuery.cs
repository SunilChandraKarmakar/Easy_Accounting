namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Queries
{
    public class SelectListParentCategoryQuery : IRequest<IEnumerable<SelectModel>>
    {
        public class Handler : IRequestHandler<SelectListParentCategoryQuery, IEnumerable<SelectModel>>
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

            public async Task<IEnumerable<SelectModel>> Handle(SelectListParentCategoryQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the login user info from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                var getCategories = await _categoryRepository.GetParentCategorySelectList(userId, cancellationToken);
                return getCategories;
            }
        }
    }
}