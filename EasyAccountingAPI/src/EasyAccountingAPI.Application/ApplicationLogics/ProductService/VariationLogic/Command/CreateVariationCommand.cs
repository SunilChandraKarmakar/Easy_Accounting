namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.VariationLogic.Command
{
    public class CreateVariationCommand : VariationCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateVariationCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IVariationRepository _variationRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IVariationRepository variationRepository,
                IUnitOfWorkRepository unitOfWorkRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _variationRepository = variationRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateVariationCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Create variation
                var variation = _mapper.Map<Variation>(request);
                variation.CreatedById = userId;
                variation.CreatedDateTime = DateTime.UtcNow;

                await _variationRepository.CreateAsync(variation, cancellationToken);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                return variation.Id > 0;
            }
        }
    }
}