namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Command
{
    public class CreateModuleSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateModuleSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IModuleRepository _moduleRepository;

            public Handler(IUnitOfWorkRepository unitOfWork, IModuleRepository moduleRepository)
            {
                _unitOfWork = unitOfWork;
                _moduleRepository = moduleRepository;
            }

            public async Task<bool> Handle(CreateModuleSeedCommand request, CancellationToken ct)
            {
                // Prevent duplicate seeding
                if (await _moduleRepository.AnyAsync(ct))
                    return true;

                // Seed modules
                var modules = GetModules();

                // Transactional operation
                await _unitOfWork.BeginTransactionAsync(ct);

                try
                {
                    await _moduleRepository.BulkCreateAsync(modules, ct);
                    await _unitOfWork.SaveChangesAsync(ct);
                    await _unitOfWork.CommitTransactionAsync(ct);

                    return true;
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(ct);
                    throw;
                }
            }

            // Master module list
            private static List<EasyAccountingAPI.Model.MasterSettings.Module> GetModules()
            {
                return new List<EasyAccountingAPI.Model.MasterSettings.Module>
                {
                    new() { Name = "Master Setting" },
                    new() { Name = "Sales & Payment" },
                    new() { Name = "Purchase" },
                    new() { Name = "Product & Service" },
                    new() { Name = " Accounting" },
                    new() { Name = "Reports" }
                }
                .Select(c => new EasyAccountingAPI.Model.MasterSettings.Module
                {
                    Name = c.Name.Trim(),
                    IsDeleted = false,
                    DeletedDateTime = null
                })
                .ToList();
            }
        }
    }
}