namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Queries
{
    public class GetCategoryDetailQuery : IRequest<CategoryUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetCategoryDetailQuery, CategoryUpdateModel>
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

            public async Task<CategoryUpdateModel> Handle(GetCategoryDetailQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Check if the category id is null, empty, whitespace, or equals to -1
                if (string.IsNullOrEmpty(request.Id) || string.IsNullOrWhiteSpace(request.Id) || request.Id == "-1")
                    return new CategoryUpdateModel();

                // Decrypt the category id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var categoryId))
                    return new CategoryUpdateModel();

                // Get category by id
                var getCategory = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);

                if (getCategory is null)
                    return new CategoryUpdateModel();

                // Map category
                var mapCategory = _mapper.Map<CategoryUpdateModel>(getCategory);
                return mapCategory;
            }
        }
    }
}