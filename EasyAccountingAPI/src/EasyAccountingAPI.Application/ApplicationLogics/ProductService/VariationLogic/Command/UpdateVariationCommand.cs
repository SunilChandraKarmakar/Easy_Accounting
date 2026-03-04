namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.VariationLogic.Command
{
    public class UpdateVariationCommand : VariationUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateVariationCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IVariationRepository _variationRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository,
                IVariationRepository variationRepository,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _variationRepository = variationRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateVariationCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing variation
                var getExistingVariation = await _variationRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getExistingVariation is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    _mapper.Map((VariationUpdateModel)request, getExistingVariation);                 
                    getExistingVariation.UpdatedById = userId;
                    getExistingVariation.UpdatedDateTime = DateTime.UtcNow;

                    _variationRepository.Update(getExistingVariation);
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