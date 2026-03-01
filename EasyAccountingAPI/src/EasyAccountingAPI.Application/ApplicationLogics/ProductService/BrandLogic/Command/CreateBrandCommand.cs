namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.BrandLogic.Command
{
    public class CreateBrandCommand : BrandCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateBrandCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IBrandRepository _brandRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor, 
                IBrandRepository brandRepository,
                IUnitOfWorkRepository unitOfWorkRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _brandRepository = brandRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Create brand 
                var brand = _mapper.Map<Brand>(request);
                brand.CreatedById = userId;
                brand.CreatedDateTime = DateTime.UtcNow;

                await _brandRepository.CreateAsync(brand, cancellationToken);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                return brand.Id > 0;
            }
        }
    }
}