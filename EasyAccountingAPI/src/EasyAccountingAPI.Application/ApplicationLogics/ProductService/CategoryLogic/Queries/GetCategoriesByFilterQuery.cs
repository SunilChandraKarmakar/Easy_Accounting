namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Queries
{
    public class GetCategoriesByFilterQuery : FilterPageModel, IRequest<FilterPageResultModel<CategoryGridModel>>
    {
        public class Handler : IRequestHandler<GetCategoriesByFilterQuery, FilterPageResultModel<CategoryGridModel>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                ICategoryRepository categoryRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<FilterPageResultModel<CategoryGridModel>> Handle(GetCategoriesByFilterQuery request,
                CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Get category and map to grid model
                var getCategories = await _categoryRepository.GetCategoriesByFilterAsync(request, userId, cancellationToken);
                var mapCategories = _mapper.Map<ICollection<CategoryGridModel>>(getCategories.Items);

                // Return paginated result
                return new FilterPageResultModel<CategoryGridModel>(mapCategories, getCategories.TotalCount);
            }
        }
    }
}