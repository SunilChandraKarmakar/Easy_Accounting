namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Command
{
    public class UpdateModuleCommand : ModuleUpdateModel, IRequest<bool>
    {
        public class Handler : IRequestHandler<UpdateModuleCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IModuleRepository _moduleRepository;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, IModuleRepository moduleRepository, IMapper mapper)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _moduleRepository = moduleRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
            {
                // Fetch existing module
                var getExistingModule = await _moduleRepository.GetByIdAsync(request.Id, cancellationToken);
                if (getExistingModule is null) return false;

                await _unitOfWorkRepository.BeginTransactionAsync(cancellationToken);

                try
                {
                    _mapper.Map((ModuleUpdateModel)request, getExistingModule);
                    _moduleRepository.Update(getExistingModule);
                    await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

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