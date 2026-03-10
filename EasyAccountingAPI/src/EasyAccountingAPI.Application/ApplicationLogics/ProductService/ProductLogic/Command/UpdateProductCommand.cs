namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Command
{
    public class UpdateProductCommand : ProductUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateProductCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IProductRepository _productRepository;
            private readonly IProductInventoryRepository _productInventoryRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IProductRepository productRepository,
                IProductInventoryRepository productInventoryRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _productRepository = productRepository;
                _productInventoryRepository = productInventoryRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing product
                var getProduct = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getProduct is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    // If the request indicates no product inventory but the product currently has inventories,
                    // delete those dependent entities to avoid leaving orphaned/invalid relationships.
                    if (!request.HaveProductInventory && getProduct.ProductInventories != null && getProduct.ProductInventories.Any())
                    {
                        // Copy to list to avoid modifying collection during enumeration
                        var existingInventories = getProduct.ProductInventories.ToList();
                        foreach (var inv in existingInventories)
                        {
                            _productInventoryRepository.Delete(inv);
                        }
                    }

                    _mapper.Map((ProductUpdateModel)request, getProduct);
                    getProduct.UpdatedById = userId;
                    getProduct.UpdatedDateTime = DateTime.UtcNow;

                    _productRepository.Update(getProduct);
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