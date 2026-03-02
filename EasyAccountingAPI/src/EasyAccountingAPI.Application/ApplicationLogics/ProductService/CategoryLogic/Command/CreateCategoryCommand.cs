namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Command
{
    public class CreateCategoryCommand : CategoryCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateCategoryCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICategoryRepository _categoryRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                ICategoryRepository categoryRepository,
                IUnitOfWorkRepository unitOfWorkRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _categoryRepository = categoryRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Create category
                var category = _mapper.Map<Category>(request);
                
                if(category.ParentId == null || category.ParentId <= 0)
                    category.ParentId = null;

                category.CreatedById = userId;
                category.CreatedDateTime = DateTime.UtcNow;

                await _categoryRepository.CreateAsync(category, cancellationToken);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                return category.Id > 0;
            }
        }
    }
}