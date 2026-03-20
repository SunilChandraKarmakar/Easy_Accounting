namespace EasyAccountingAPI.Repository.Repository.Purchase
{
    public class PurchaseItemRepository : BaseRepository<PurchaseItem>, IPurchaseItemRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public PurchaseItemRepository(DatabaseContext databaseContext, ICompanyRepository companyRepository) : base(databaseContext)
        {
            _companyRepository = companyRepository;
        }
    }
}