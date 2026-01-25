namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Command
{
    public class DeleteModuleCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteModuleCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWorkRepository;
            private readonly IModuleRepository _moduleRepository;

            public Handler(IUnitOfWorkRepository unitOfWorkRepository, IModuleRepository moduleRepository)
            {
                _unitOfWorkRepository = unitOfWorkRepository;
                _moduleRepository = moduleRepository;
            }

            public async Task<bool> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
            {
                // Decrypt the module id
                var decryptedId = EncryptionService.Decrypt(request.Id);
                if (!int.TryParse(decryptedId, out var moduleId))
                    return false;

                // Fetch the module
                var module = await _moduleRepository.GetByIdAsync(moduleId, cancellationToken);
                if (module is null)
                    return false;

                module.IsDeleted = true;
                module.DeletedDateTime = DateTime.UtcNow;
                _moduleRepository.Update(module);
                return await _unitOfWorkRepository.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}