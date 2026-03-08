namespace EasyAccountingAPI.Repository.Repository.ProductService
{
    public class ProductInventoryRepository : BaseRepository<ProductInventory>, IProductInventoryRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public ProductInventoryRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }
    }
}