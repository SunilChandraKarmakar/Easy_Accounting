namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Command
{
    public class CreateProductCommand : ProductCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateProductCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IProductRepository productRepository,
                IUnitOfWorkRepository unitOfWorkRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _productRepository = productRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                // Get the user ID from the HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Start a transaction
                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    var product = _mapper.Map<Product>(request);
                    product.CreatedById = userId;
                    product.CreatedDateTime = DateTime.UtcNow;
                    
                    await _productRepository.CreateAsync(product, cancellationToken);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await _unitOfWorkRepository.CommitTransactionAsync(cancellationToken);

                    return product.Id > 0;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}