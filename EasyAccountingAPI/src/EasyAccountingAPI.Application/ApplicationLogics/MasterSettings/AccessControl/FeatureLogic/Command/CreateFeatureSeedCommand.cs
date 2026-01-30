namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Command
{
    public class CreateFeatureSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateFeatureSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IModuleRepository _moduleRepository;
            private readonly IFeatureRepository _featureRepository;

            public Handler(IUnitOfWorkRepository unitOfWork, IModuleRepository moduleRepository, IFeatureRepository featureRepository)
            {
                _unitOfWork = unitOfWork;
                _moduleRepository = moduleRepository;
                _featureRepository = featureRepository;
            }

            public async Task<bool> Handle(CreateFeatureSeedCommand request, CancellationToken cancellationToken)
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Get modules and features
                    var existingModules = await _moduleRepository.GetAllAsync(cancellationToken);
                    var existingFeatures = await _featureRepository.GetAllAsync(cancellationToken);

                    var modulesToInsert = new List<EasyAccountingAPI.Model.MasterSettings.Module>();
                    var featuresToInsert = new List<Feature>();

                    var modules = GetModules();
                    var masterFeatures = GetMasterSettingFeatures();

                    // Working on the module
                    foreach (var module in modules)
                    {
                        if (!existingModules.Any(m => m.Name == module.Name))
                            modulesToInsert.Add(module);
                    }

                    if (modulesToInsert.Any())
                        await _moduleRepository.BulkCreateAsync(modulesToInsert, cancellationToken);

                    // Save once so ids are generated
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    // Refresh modules dictionary
                    var moduleDict = existingModules
                        .Concat(modulesToInsert)
                        .ToDictionary(m => m.Name, m => m.Id);

                    // Working on the features
                    foreach (var feature in masterFeatures)
                    {
                        if (!existingFeatures.Any(f => f.TableName == feature.TableName))
                        {
                            feature.ModuleId = moduleDict["Master Setting"];
                            featuresToInsert.Add(feature);
                        }
                    }

                    if (featuresToInsert.Any())
                        await _featureRepository.BulkCreateAsync(featuresToInsert, cancellationToken);

                    // One save, one commit
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return true;
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw; 
                }
            }

            // Get modules
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

            private ICollection<Feature> GetMasterSettingFeatures()
            {
                return new List<Feature>()
                {                
                    new Feature { Code = "Country", Name = "Country", TableName = "Countries", ControllerName = "Country" },
                    new Feature { Code = "City", Name = "City", TableName = "Cities", ControllerName = "City" },
                    new Feature { Code = "Currency", Name = "Currency", TableName = "Currencies", ControllerName = "Currency" },
                    new Feature { Code = "Module", Name = "Module", TableName = "Modules", ControllerName = "Module" },
                    new Feature { Code = "Company", Name = "Company", TableName = "Companies", ControllerName = "Company" },
                    new Feature { Code = "InvoiceSetting", Name = "Invoice Setting", TableName = "InvoiceSettings", 
                        ControllerName = "InvoieSetting" },
                    new Feature { Code = "Action", Name = "Action", TableName = "Actions", ControllerName = "Action" }
                };
            }
        }
    }
}