namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.FeatureLogic.Command
{
    public class CreateFeatureSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateFeatureSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IModuleRepository _moduleRepository;
            private readonly IFeatureRepository _featureRepository;

            public Handler(
                IUnitOfWorkRepository unitOfWork,
                IModuleRepository moduleRepository,
                IFeatureRepository featureRepository)
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
                    // Get existing data
                    var existingModules = await _moduleRepository.GetAllAsync(cancellationToken);
                    var existingFeatures = await _featureRepository.GetAllAsync(cancellationToken);

                    // Create lookup for existing modules and features
                    var existingModuleDict = existingModules.ToDictionary(m => m.Name, m => m);

                    // Use HashSet for O(1) lookups on existing feature table names
                    var existingFeatureTableSet = existingFeatures
                        .Select(f => f.TableName)
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    // Prepare modules
                    var seedModules = GetModules();

                    // Only insert modules that don't exist
                    var modulesToInsert = seedModules
                        .Where(m => !existingModuleDict.ContainsKey(m.Name))
                        .ToList();

                    if (modulesToInsert.Any())
                        await _moduleRepository.BulkCreateAsync(modulesToInsert, cancellationToken);

                    // Save once to generate IDs
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    // Refresh module dictionary (after insert)
                    var allModules = existingModules.Concat(modulesToInsert).ToDictionary(m => m.Name, m => m.Id);

                    // Prepare features dynamically
                    var moduleFeatureMap = GetModuleFeatureMap();
                    var featuresToInsert = new List<Feature>();

                    foreach (var moduleEntry in moduleFeatureMap)
                    {
                        if (!allModules.TryGetValue(moduleEntry.Key, out var moduleId))
                            continue;

                        foreach (var feature in moduleEntry.Value)
                        {
                            if (!existingFeatureTableSet.Contains(feature.TableName))
                            {
                                feature.ModuleId = moduleId;
                                feature.IsDeleted = false;
                                feature.DeletedDateTime = null;

                                featuresToInsert.Add(feature);
                            }
                        }
                    }

                    if (featuresToInsert.Any())
                        await _featureRepository.BulkCreateAsync(featuresToInsert, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return modulesToInsert.Any() || featuresToInsert.Any();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }

            private static List<EasyAccountingAPI.Model.MasterSettings.Module> GetModules()
            {
                return new List<EasyAccountingAPI.Model.MasterSettings.Module>
                {
                    new() { Name = "Master Setting" },
                    new() { Name = "Sales & Payment" },
                    new() { Name = "Purchase" },
                    new() { Name = "Product & Service" },
                    new() { Name = "Accounting" },
                    new() { Name = "Reports" }
                }
                .Select(m => new EasyAccountingAPI.Model.MasterSettings.Module
                {
                    Name = m.Name.Trim(),
                    IsDeleted = false,
                    DeletedDateTime = null
                })
                .ToList();
            }

            private static Dictionary<string, List<Feature>> GetModuleFeatureMap()
            {
                return new Dictionary<string, List<Feature>>
                {
                    ["Master Setting"] = new()
                    {
                        new() { Code = "Country", Name = "Country", TableName = "Countries", ControllerName = "Country" },
                        new() { Code = "City", Name = "City", TableName = "Cities", ControllerName = "City" },
                        new() { Code = "Currency", Name = "Currency", TableName = "Currencies", ControllerName = "Currency" },
                        new() { Code = "Module", Name = "Module", TableName = "Modules", ControllerName = "Module" },
                        new() { Code = "Company", Name = "Company", TableName = "Companies", ControllerName = "Company" },
                        new() { Code = "InvoiceSetting", Name = "Invoice Setting", TableName = "InvoiceSettings", ControllerName = "InvoiceSetting" },
                        new() { Code = "Action", Name = "Action", TableName = "Actions", ControllerName = "Action" }
                    }
                };
            }
        }
    }
}