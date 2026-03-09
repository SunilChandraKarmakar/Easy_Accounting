namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Command
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteProductCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IProductRepository _productRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IProductRepository productRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _productRepository = productRepository;
            }

            public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the product id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var productId))
                    return false;

                // Fetch the product
                var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
                if (product is null)
                    return false;

                product.IsDeleted = true;
                product.DeletedDateTime = DateTime.UtcNow;

                // Update the product inventory
                foreach (var inventory in product.ProductInventories)
                {
                    inventory.IsDeleted = true;
                    inventory.DeletedDateTime = DateTime.UtcNow;
                }

                _productRepository.Update(product);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}