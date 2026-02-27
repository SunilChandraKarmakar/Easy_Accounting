namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Command
{
    public class DeleteProductUnitCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteProductUnitCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IProductUnitRepository _productUnitRepository;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository, 
                IProductUnitRepository productRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _productUnitRepository = productRepository;
            }

            public async Task<bool> Handle(DeleteProductUnitCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Decrypt the product unit id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var productUnitId))
                    return false;

                // Fetch the product unit
                var productUnit = await _productUnitRepository.GetByIdAsync(productUnitId, cancellationToken);
                if (productUnit is null)
                    return false;

                productUnit.IsDeleted = true;
                productUnit.DeletedDateTime = DateTime.UtcNow;

                _productUnitRepository.Update(productUnit);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}