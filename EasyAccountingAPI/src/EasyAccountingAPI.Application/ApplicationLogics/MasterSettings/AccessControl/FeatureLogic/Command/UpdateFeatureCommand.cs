namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Command
{
    public class UpdateFeatureCommand : FeatureUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateFeatureCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IFeatureRepository _featureRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, IFeatureRepository featureRepository,
                IHttpContextAccessor httpContextAccessor, IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _featureRepository = featureRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateFeatureCommand request, CancellationToken ct)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Fetch existing feature
                var getExistingFeature = await _featureRepository.GetByIdAsync(request.Id, ct);
                if (getExistingFeature is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(ct);

                try
                {
                    _mapper.Map((FeatureUpdateModel)request, getExistingFeature);
                    _featureRepository.Update(getExistingFeature);
                    await _unitOfWorkRepository.SaveChangesAsync(ct);
                    await _unitOfWorkRepository.CommitTransactionAsync(ct);

                    return true;
                }
                catch
                {
                    await _unitOfWorkRepository.RollbackTransactionAsync(ct);
                    return false;
                }
            }
        }
    }
}