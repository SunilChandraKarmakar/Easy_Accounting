namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Command
{
    public class CreateProductUnitSeedCommand : IRequest<bool>
    {
        public sealed class Handler : IRequestHandler<CreateProductUnitSeedCommand, bool>
        {
            private readonly IUnitOfWorkRepository _unitOfWork;
            private readonly IProductUnitRepository _productUnitRepository;

            public Handler(
                IUnitOfWorkRepository unitOfWork, 
                IProductUnitRepository productUnitRepository)
            {
                _unitOfWork = unitOfWork;
                _productUnitRepository = productUnitRepository;
            }

            public async Task<bool> Handle(CreateProductUnitSeedCommand request, CancellationToken ct)
            {
                var newCreatedProductUnits = new List<ProductUnit>();   

                // Seed product units
                var seedProductUnits = GetProductUnits();

                // Get product units that already exist in the database
                var existingProductUnits = await _productUnitRepository.GetAllAsync(ct);

                // Filter out product units that already exist and create new ones
                newCreatedProductUnits = seedProductUnits
                    .Where(s => !existingProductUnits.Any(e => e.Name.Equals(s, StringComparison.OrdinalIgnoreCase)))
                    .Select(s => new ProductUnit 
                    { 
                        Name = s,
                        CreatedById = "System", 
                        CreatedDateTime = DateTime.UtcNow
                    })
                    .ToList();

                // Transactional operation
                await _unitOfWork.BeginTransactionAsync(ct);

                try
                {
                    await _productUnitRepository.BulkCreateAsync(newCreatedProductUnits, ct);
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

            // Product units
            private static string[] GetProductUnits()
            {
                return new[]
                {
                    "Piece",
                    "Box",
                    "Kilogram",
                    "Liter",
                    "Meter",
                    "Pack",
                    "Set",
                    "Dozen",
                    "Gram",
                    "Milligram",
                    "Milliliter",
                    "Centimeter",
                    "Foot",
                    "Inch",
                    "Meter",
                    "Kilometer",
                    "Pound",
                    "Yard"
                };
            }
        }
    }
}