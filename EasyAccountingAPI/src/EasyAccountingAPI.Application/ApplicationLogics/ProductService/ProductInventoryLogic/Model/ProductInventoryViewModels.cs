namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductInventoryLogic.Model
{
    public class ProductInventoryViewModel
    {
        public ProductInventoryCreateModel CreateModel { get; set; }
        public ProductInventoryUpdateModel UpdateModel { get; set; }
        public ProductInventoryGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class ProductInventoryCreateModel : IMapFrom<ProductInventory>
    {
        public int ProductId { get; set; }
        public decimal OpeningStock { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool HaveStockAlert { get; set; }
        public decimal? StockAlertQty { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductInventoryCreateModel, ProductInventory>();
        }
    }

    public class ProductInventoryUpdateModel : IMapFrom<ProductInventory>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal OpeningStock { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool HaveStockAlert { get; set; }
        public decimal? StockAlertQty { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductInventory, ProductInventoryUpdateModel>();
            profile.CreateMap<ProductInventoryUpdateModel, ProductInventory>();
        }
    }

    public class ProductInventoryGridModel : IMapFrom<ProductInventory>
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal OpeningStock { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool HaveStockAlert { get; set; }
        public decimal? StockAlertQty { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductInventory, ProductInventoryGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())) )
                .ForMember(d => d.ProductName, s => s.MapFrom(m => m.Product.Name));
        }
    }
}