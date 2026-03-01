namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.BrandLogic.Command
{
    public class UpdateBrandCommand : BrandUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateBrandCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUnitOfWorkRepository unitOfWorkRepository, 
                IBrandRepository brandRepository, 
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWorkRepository = unitOfWorkRepository;
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing brand
                var getExistingBrand = await _brandRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getExistingBrand is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    _mapper.Map((BrandUpdateModel)request, getExistingBrand);
                    getExistingBrand.UpdatedById = userId;
                    getExistingBrand.UpdatedDateTime = DateTime.UtcNow;

                    _brandRepository.Update(getExistingBrand);
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