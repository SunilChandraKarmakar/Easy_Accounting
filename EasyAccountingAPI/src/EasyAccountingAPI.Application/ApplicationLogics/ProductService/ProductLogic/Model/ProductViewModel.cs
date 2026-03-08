namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.ProductLogic.Model
{
    public class ProductViewModel
    {
        public ProductCreateModel CreateModel { get; set; }
        public ProductUpdateModel UpdateModel { get; set; }
        public ProductGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class ProductCreateModel : IMapFrom<Product>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProductUnitId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int CompanyId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int? VatTaxId { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool HaveProductInventory { get; set; }

        public ProductInventoryCreateModel ProductInventory { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductCreateModel, Product>()
                .ForMember(d => d.ProductInventories, s => s.MapFrom(m => m.ProductInventory == null ? Enumerable.Empty<ProductInventoryCreateModel>() : new[] { m.ProductInventory }));
        }
    }

    public class ProductUpdateModel : IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProductUnitId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int CompanyId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int? VatTaxId { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool HaveProductInventory { get; set; }

        public ProductInventoryUpdateModel ProductInventory { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductUpdateModel>()
                .ForMember(d => d.ProductInventory, s => s.MapFrom(m => m.ProductInventories.FirstOrDefault()));
            profile.CreateMap<ProductUpdateModel, Product>()
                .ForMember(d => d.ProductInventories, s => s.MapFrom(m => m.ProductInventory == null ? Enumerable.Empty<ProductInventoryUpdateModel>() : new[] { m.ProductInventory }));
        }
    }

    public class ProductGridModel : IMapFrom<Product>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProductUnitName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string CompanyName { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public string? VatTaxName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool HaveProductInventory { get; set; }

        public ProductInventoryGridModel ProductInventory { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductGridModel>()
                .ForMember(d => d.Id, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.ProductUnitName, s => s.MapFrom(m => m.ProductUnit.Name))
                .ForMember(d => d.CategoryName, s => s.MapFrom(m => m.Category.Name))
                .ForMember(d => d.BrandName, s => s.MapFrom(m => m.Brand.Name))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name))
                .ForMember(d => d.VatTaxName, s => s.MapFrom(m => m.VatTax != null ? m.VatTax.TaxName : null))
                .ForMember(d => d.ProductInventory, s => s.MapFrom(m => m.ProductInventories.FirstOrDefault()));
        }
    }
}