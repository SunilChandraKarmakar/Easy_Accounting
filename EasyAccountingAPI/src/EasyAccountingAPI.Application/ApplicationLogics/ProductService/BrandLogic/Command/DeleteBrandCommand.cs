namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.BrandLogic.Command
{
    public class DeleteBrandCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteBrandCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IBrandRepository _repositoryRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository, 
                IBrandRepository brandRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _repositoryRepository = brandRepository;
            }

            public async Task<bool> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the brand id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var brandId))
                    return false;

                // Fetch the brand
                var brand = await _repositoryRepository.GetByIdAsync(brandId, cancellationToken);
                if (brand is null)
                    return false;

                brand.IsDeleted = true;
                brand.DeletedDateTime = DateTime.UtcNow;

                _repositoryRepository.Update(brand);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}