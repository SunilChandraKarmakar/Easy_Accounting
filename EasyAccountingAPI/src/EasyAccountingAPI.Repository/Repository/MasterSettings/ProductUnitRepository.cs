namespace EasyAccountingAPI.Repository.Repository.MasterSettings
{
    public class ProductUnitRepository : BaseRepository<ProductUnit>, IProductUnitRepository
    {
        public ProductUnitRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public override async Task<ICollection<ProductUnit>> GetAllAsync(CancellationToken cancellationToken)
        {
            var productUnits = db.ProductUnits
                .Where(pu => !pu.IsDeleted);

            return await productUnits.ToListAsync(cancellationToken);
        }

        public override async Task<ProductUnit?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var productUnit = await db.ProductUnits
                .Where(pu => pu.Id == id && !pu.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            return productUnit;
        }

        // Get product unit with filtering, sorting, and pagination
        public async Task<FilterPageResultModel<ProductUnit>> GetProductUnitsByFilterAsync(FilterPageModel model,
            CancellationToken cancellationToken)
        {
            Expression<Func<ProductUnit, bool>> filter = pu =>
                 !pu.IsDeleted
                 && (string.IsNullOrWhiteSpace(model.FilterValue)
                 || pu.Name.Contains(model.FilterValue));

            var sortableColumns = new Dictionary<string, Expression<Func<ProductUnit, object>>>
            {
                ["name"] = pu => pu.Name,
                ["id"] = pu => pu.Id
            };

            return await GetAllFilterAsync(model, filter, vt => vt.Id, sortableColumns, include: null, cancellationToken);
        }

        public async Task<IEnumerable<SelectModel>> GetProductUnitSelectListAsync(CancellationToken cancellationToken)
        {
            var productUnits = db.ProductUnits
                .Where(pu => !pu.IsDeleted)
                .Select(pu => new SelectModel
                {
                    Id = pu.Id,
                    Name = pu.Name
                });

            return await productUnits.ToListAsync(cancellationToken);
        }
    }
}