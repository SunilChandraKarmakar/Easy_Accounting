namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Queries
{
    public class GetModuleDetailQuery : IRequest<ModuleUpdateModel>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<GetModuleDetailQuery, ModuleUpdateModel>
        {
            private readonly IModuleRepository _moduleRepository;
            private readonly IMapper _mapper;

            public Handler(IModuleRepository moduleRepository, IMapper mapper)
            {
                _moduleRepository = moduleRepository;
                _mapper = mapper;
            }

            public async Task<ModuleUpdateModel> Handle(GetModuleDetailQuery request, CancellationToken cancellationToken)
            {
                // Decrypt the module id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var moduleId))
                    return new ModuleUpdateModel();

                // Get module by id
                var getModule = await _moduleRepository.GetByIdAsync(moduleId, cancellationToken);

                if (getModule is null)
                    return new ModuleUpdateModel();

                // Map module
                var mapModule = _mapper.Map<ModuleUpdateModel>(getModule);
                return mapModule;
            }
        }
    }
}