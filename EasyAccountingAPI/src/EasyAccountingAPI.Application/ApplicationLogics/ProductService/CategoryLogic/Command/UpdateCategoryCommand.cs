namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Command
{
    public class UpdateCategoryCommand : CategoryUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateCategoryCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                ICategoryRepository categoryRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing category
                var getExistingCategory = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getExistingCategory is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    _mapper.Map((CategoryUpdateModel)request, getExistingCategory);

                    if(getExistingCategory.ParentId == null || getExistingCategory.ParentId <= 0)
                        getExistingCategory.ParentId = null;

                    getExistingCategory.UpdatedById = userId;
                    getExistingCategory.UpdatedDateTime = DateTime.UtcNow;

                    _categoryRepository.Update(getExistingCategory);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    return false;
                }
            }
        }
    }
}