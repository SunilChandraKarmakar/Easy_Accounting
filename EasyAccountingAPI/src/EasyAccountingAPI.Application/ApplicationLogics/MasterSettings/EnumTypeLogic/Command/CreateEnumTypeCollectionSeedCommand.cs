namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EnumTypeLogic.Command
{
    public class CreateEnumTypeCollectionSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateEnumTypeCollectionSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IEnumTypeRepository _enumTypeRepository;
            private readonly IEnumTypeCollectionRepository _enumTypeCollectionRepository;

            public Handler(
                IUnitOfWorkRepository unitOfWork,
                IEnumTypeRepository enumTypeRepository,
                IEnumTypeCollectionRepository enumTypeCollectionRepository)
            {
                _unitOfWork = unitOfWork;
                _enumTypeRepository = enumTypeRepository;
                _enumTypeCollectionRepository = enumTypeCollectionRepository;
            }

            public async Task<bool> Handle(CreateEnumTypeCollectionSeedCommand request, CancellationToken cancellationToken)
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    // Get existing data
                    var existingEnumTypes = await _enumTypeRepository.GetAllAsync(cancellationToken);
                    var existingEnumCollections = await _enumTypeCollectionRepository.GetAllAsync(cancellationToken);

                    var existingEnumTypeDict = existingEnumTypes
                        .ToDictionary(e => e.Name, e => e);

                    var existingCollectionIdSet = existingEnumCollections
                        .Select(c => c.Id)
                        .ToHashSet();

                    // Seed Enum Types
                    var seedEnumTypes = GetEnumTypes();

                    var enumTypesToInsert = seedEnumTypes
                        .Where(e => !existingEnumTypeDict.ContainsKey(e.Name))
                        .ToList();

                    if (enumTypesToInsert.Any())
                        await _enumTypeRepository.BulkCreateAsync(enumTypesToInsert, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    // Refresh dictionary after insert
                    var allEnumTypes = existingEnumTypes
                        .Concat(enumTypesToInsert)
                        .ToDictionary(e => e.Name, e => e.Id);

                    // Seed Enum Type Collections
                    var enumTypeCollectionMap = GetEnumTypeCollectionMap();
                    var collectionsToInsert = new List<EnumTypeCollection>();

                    foreach (var enumTypeEntry in enumTypeCollectionMap)
                    {
                        if (!allEnumTypes.TryGetValue(enumTypeEntry.Key, out var enumTypeId))
                            continue;

                        foreach (var collection in enumTypeEntry.Value)
                        {
                            if (!existingCollectionIdSet.Contains(collection.Id))
                            {
                                collection.EnumTypeId = enumTypeId;
                                collection.IsDeleted = false;

                                collectionsToInsert.Add(collection);
                            }
                        }
                    }

                    if (collectionsToInsert.Any())
                        await _enumTypeCollectionRepository.BulkCreateAsync(collectionsToInsert, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return enumTypesToInsert.Any() || collectionsToInsert.Any();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }

            // Enum Type Seed
            private static List<EnumType> GetEnumTypes()
            {
                return new List<EnumType>
                {
                    new() { Id = 1, Name = "Global Status" },
                    new() { Id = 2, Name = "Payment Status" }
                };
            }

            // Enum Type Collection 
            private static Dictionary<string, List<EnumTypeCollection>> GetEnumTypeCollectionMap()
            {
                return new Dictionary<string, List<EnumTypeCollection>>
                {
                    ["Global Status"] = new()
                    {
                        new() { Id = 1, Name = "Active", EnumTypeId = 1 },
                        new() { Id = 2, Name = "Inactive", EnumTypeId = 1 }
                    },

                    ["Payment Status"] = new()
                    {
                        new() { Id = 3, Name = "Paid", EnumTypeId = 2 },
                        new() { Id = 4, Name = "Partial", EnumTypeId = 2 },
                        new() { Id = 5, Name = "Unpaid", EnumTypeId = 2 }
                    }
                };
            }
        }
    }
}