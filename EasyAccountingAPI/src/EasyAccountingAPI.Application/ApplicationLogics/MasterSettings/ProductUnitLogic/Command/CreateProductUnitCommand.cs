namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Command
{
    public class CreateProductUnitCommand : ProductUnitCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateProductUnitCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IProductUnitRepository _productUnitRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor, 
                IProductUnitRepository productUnitRepository,
                IUnitOfWorkRepository unitOfWorkRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _productUnitRepository = productUnitRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateProductUnitCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Create product unit entity
                var productUnit = _mapper.Map<ProductUnit>(request);
                productUnit.CreatedById = userId;
                productUnit.CreatedDateTime = DateTime.UtcNow;

                await _productUnitRepository.CreateAsync(productUnit, cancellationToken);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                return productUnit.Id > 0;
            }
        }
    }
}