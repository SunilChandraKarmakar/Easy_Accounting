namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Command
{
    public class CreateModuleCommand : ModuleCreateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<CreateModuleCommand, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IModuleRepository _moduleRepository;
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IMapper _mapper;

            public Handler(IHttpContextAccessor httpContextAccessor, IModuleRepository moduleRepository, 
                IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _moduleRepository = moduleRepository;
                _unitOfWorkRepository = unitOfWorkRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
            {
                // Retrieve the user's Id from the current HTTP context
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                // Check if the user Id is null or not
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException(ProvideErrorMessage.UserNotAuthenticated);

                // Create module entity
                var module = _mapper.Map<EasyAccountingAPI.Model.MasterSettings.Module>(request);                
                await _moduleRepository.CreateAsync(module, cancellationToken);
                await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

                return module.Id > 0;
            }
        }
    }
}