namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Command
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteCategoryCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly ICategoryRepository _categoryRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                ICategoryRepository categoryRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _categoryRepository = categoryRepository;
            }

            public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the category id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var categoryId))
                    return false;

                // Fetch the category
                var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
                if (category is null)
                    return false;

                category.IsDeleted = true;
                category.DeletedDateTime = DateTime.UtcNow;

                _categoryRepository.Update(category);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}