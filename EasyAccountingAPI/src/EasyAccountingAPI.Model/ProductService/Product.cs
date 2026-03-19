namespace EasyAccountingAPI.Model.ProductService
{
    [Table("Products", Schema = "ProductService")]
    public class Product : IAuditableEntity, IDelatableEntity
    {
        public Product()
        {
            ProductInventories = new HashSet<ProductInventory>();
            PurchaseItems = new HashSet<PurchaseItem>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product code is required.")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Product code must be between 6 and 40 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Product unit is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product unit id must be greater than 0.")]
        public int ProductUnitId { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Category id must be greater than 0.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Brand id must be greater than 0.")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Company id must be greater than 0.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0.")]
        public decimal CostPrice { get; set; }

        [Required(ErrorMessage = "Product sell price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product sell price must be greater than 0.")]
        public decimal SellPrice { get; set; }

        [Required(ErrorMessage = "Please, provide total quantity")]
        public int TotalQuantity { get; set; }

        public int? VatTaxId { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }        
        public bool HaveProductInventory { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public ProductUnit ProductUnit { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public Company Company { get; set; }
        public VatTax? VatTax { get; set; }
        public ICollection<ProductInventory> ProductInventories { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}